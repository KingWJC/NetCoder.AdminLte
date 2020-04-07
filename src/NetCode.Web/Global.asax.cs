using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using AutoMapper;
using DotLiquid.ViewEngine;
using NetCode.Business.Base_SysManage;
using NetCode.DataRepository;
using NetCode.Entity.Base_SysManage;
using NetCode.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebLiquid.Filters;

namespace NetCode.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            InitAutofac();
            InitEF();
        }

        /// <summary>
        /// 初始化依赖注入
        /// </summary>
        private void InitAutofac()
        {
            var builder = new ContainerBuilder();

            var baseType = typeof(IDependency);
            var baseTypeCircle = typeof(ICircleDependency);

            //NetCode相关程序集
            var assemblys = BuildManager.GetReferencedAssemblies().Cast<Assembly>()
                .Where(x => x.FullName.Contains("NetCode")).ToList();

            //自动注入IDependency接口,支持AOP,生命周期为InstancePerDependency
            builder.RegisterAssemblyTypes(assemblys.ToArray())
                .Where(x => baseType.IsAssignableFrom(x) && x != baseType)
                .AsImplementedInterfaces()
                .PropertiesAutowired()
                .InstancePerDependency()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(Interceptor));

            //自动注入ICircleDependency接口,循环依赖注入,不支持AOP,生命周期为InstancePerLifetimeScope
            builder.RegisterAssemblyTypes(assemblys.ToArray())
                .Where(x => baseTypeCircle.IsAssignableFrom(x) && x != baseTypeCircle)
                .AsImplementedInterfaces()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope();

            //注册Controller
            builder.RegisterControllers(assemblys.ToArray())
                .PropertiesAutowired();
            //注册Filter
            builder.RegisterFilterProvider();
            //注册View
            builder.RegisterSource(new ViewRegistrationSource());

            //注册API-Controller
            builder.RegisterApiControllers(assemblys.ToArray())
    .PropertiesAutowired();
            //注册API-Filter
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

            //AOP
            builder.RegisterType<Interceptor>();

            //AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Base_User, Base_UserDTO>();
                cfg.CreateMap<Base_Role, Base_RoleDTO>();
            });
            var mapper = config.CreateMapper();
            builder.RegisterInstance(mapper);

            var container = builder.Build();
            //设置MVC的依赖关系解析程序
            var mvcResolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(mvcResolver);
            //设置WebAPI的依赖关系解析程序
            var webApiResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = webApiResolver;

            //初始化容器
            AutofacHelper.Container = container;
        }

        /// <summary>
        /// EF预热(映射初始化)
        /// </summary>
        private void InitEF()
        {
            Task.Run(() =>
            {
                var db = DbFactory.GetRepository();

                db.GetIQueryable<Base_User>().ToList();
            });
        }

        /// <summary>
        /// 初始化DotLiquid模板引擎
        /// </summary>
        private void InitDotLiquid()
        {
            ViewEngines.Engines.Add(new DotLiquidViewEngine());
            RegisterFilter.Register();
            RegisterFilter.RegisterType<Base_UserDTO>();
            RegisterFilter.RegisterType<Base_Menu>();
            RegisterFilter.RegisterType<CookieLoginContext>();
        }
    }
}
