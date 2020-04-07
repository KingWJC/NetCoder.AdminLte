using NetCode.Entity;
using NetCode.Util;
using System;
using System.Collections.Generic;

namespace NetCode.Business
{
    public interface ILogSearcher
    {
        List<Base_SysLog> GetLogList(
            Pagination pagination,
            string logContent,
            string logType,
            string level,
            string opUserName,
            DateTime? startTime,
            DateTime? endTime);
    }
}
