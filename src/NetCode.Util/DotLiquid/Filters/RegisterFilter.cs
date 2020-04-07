using DotLiquid;

namespace WebLiquid.Filters
{
    public class RegisterFilter
    {
        public static void Register()
        {
            DotLiquid.Template.RegisterFilter(typeof(DotliquidJsonFilter));
        }

        public static void RegisterType<T>() where T : class,new()
        {
            Template.RegisterSafeType(typeof(T),
                Hash.FromAnonymousObject);
        }
    }
}
