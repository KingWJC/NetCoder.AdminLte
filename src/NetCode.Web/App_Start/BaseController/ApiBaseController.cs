using NetCode.Util;
using System.Web.Http;

namespace NetCode.Web
{
    /// <summary>
    /// WebAPI接口基控制器
    /// </summary>
    [CheckSign]
    public class ApiBaseController : ApiController
    {
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns></returns>
        public AjaxResult Success()
        {
            return new AjaxResult
            {
                Success = true,
                ErrorCode = 0,
                Msg = "请求成功！",
                Data = null
            };
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public AjaxResult Success(string msg)
        {
            return new AjaxResult
            {
                Success = true,
                ErrorCode = 0,
                Msg = msg,
                Data = null
            };
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name = "data" > 返回的数据 </ param >
        /// < returns ></ returns >
        public AjaxResult Success<T>(T data)
        {
            return new AjaxResult
            {
                Success = true,
                ErrorCode = 0,
                Msg = "请求成功！",
                Data = data
            };
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">返回的消息</param>
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        public AjaxResult Success(string msg, object data)
        {
            return new AjaxResult
            {
                Success = true,
                ErrorCode = 0,
                Msg = msg,
                Data = data
            };
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <returns></returns>
        public AjaxResult Error()
        {
            return new AjaxResult
            {
                Success = false,
                ErrorCode = 1,
                Msg = "请求失败！",
                Data = null
            };
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="msg">错误提示</param>
        /// <returns></returns>
        public AjaxResult Error(string msg)
        {
            return new AjaxResult
            {
                Success = false,
                Msg = msg,
                ErrorCode = 1,
                Data = null
            };
        }

        public AjaxResult Error(string msg, int code)
        {
            return new AjaxResult
            {
                Success = false,
                Msg = msg,
                ErrorCode = code,
                Data = null
            };
        }
    }
}