using System;
namespace Entities
{
    public class ResponseResultFactory
    {
        public static ResponseResult Create(ErrorCode returnCode)
        {
            return new ResponseResult() { ErrorCode = returnCode, ErrorMsg = returnCode.ToString() };
        }

        public static ResponseResult<T> Create<T>(ErrorCode returnCode, T data)
        {
            return new ResponseResult<T>() { ErrorCode = returnCode, ErrorMsg = returnCode.ToString(), Data = data };
        }

        public static ResponseResult ErrorParamsHasEmpty = new ResponseResult()
        {
            ErrorCode = ErrorCode.参数缺失,
            ErrorMsg = ErrorCode.参数缺失.ToString()
        };

        public static ResponseResult ErrorParams = new ResponseResult()
        {
            ErrorCode = ErrorCode.参数有误,
            ErrorMsg = ErrorCode.参数有误.ToString()
        };

        public static ResponseResult Success = new ResponseResult()
        {
            ErrorCode = ErrorCode.请求成功,
            ErrorMsg = ErrorCode.请求成功.ToString()
        };
    }
}
