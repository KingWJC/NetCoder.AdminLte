using NetCode.Util;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace NetCode.Business
{
    public abstract class WriteDataLogAttribute : BaseFilterAttribute
    {
        public WriteDataLogAttribute(LogType logType, string nameField, string dataName)
        {
            _logType = logType;
            _dataName = dataName;
            _nameField = nameField;
        }
        protected LogType _logType { get; }
        protected string _dataName { get; }
        protected string _nameField { get; }
        protected Type _entityType { get; }
        protected ILogger Logger { get; } = AutofacHelper.GetService<ILogger>();
    }

    public class DataAddLogAttribute : WriteDataLogAttribute
    {
        public DataAddLogAttribute(LogType logType, string nameField, string dataName)
            : base(logType, nameField, dataName)
        {
        }

        public override void OnActionExecuting(IInvocation invocation)
        {

        }

        public override void OnActionExecuted(IInvocation invocation)
        {
            if ((invocation.ReturnValue as AjaxResult).Success)
            {
                var obj = invocation.Arguments[0];
                Logger.Info(_logType, $"添加{_dataName}:{obj.GetPropertyValue(_nameField)?.ToString()}");
            }
        }
    }

    class DataEditLogAttribute : WriteDataLogAttribute
    {
        public DataEditLogAttribute(LogType logType, string nameField, string dataName)
            : base(logType, nameField, dataName)
        {
        }

        public override void OnActionExecuting(IInvocation invocation)
        {

        }

        public override void OnActionExecuted(IInvocation invocation)
        {
            if ((invocation.ReturnValue as AjaxResult).Success)
            {
                var obj = invocation.Arguments[0];
                Logger.Info(_logType, $"修改{_dataName}:{obj.GetPropertyValue(_nameField)?.ToString()}");
            }
        }
    }

    public class DataDeleteLogAttribute : WriteDataLogAttribute
    {
        public DataDeleteLogAttribute(LogType logType, string nameField, string dataName)
            : base(logType, nameField, dataName)
        {
        }

        private List<object> _deleteList { get; set; }

        public override void OnActionExecuting(IInvocation invocation)
        {
            List<string> ids = invocation.Arguments[0] as List<string>;
            var q = invocation.InvocationTarget.GetType().GetMethod("GetIQueryable").Invoke(invocation.InvocationTarget, new object[] { }) as IQueryable;
            _deleteList = q.Where("@0.Contains(outerIt.Id)", ids).CastToList<object>();
        }

        public override void OnActionExecuted(IInvocation invocation)
        {
            if ((invocation.ReturnValue as AjaxResult).Success)
            {
                string names = string.Join(",", _deleteList.Select(x => x.GetPropertyValue(_nameField)?.ToString()));
                Logger.Info(_logType, $"删除{_dataName}:{names}", _deleteList.ToJson());
            }
        }
    }
}
