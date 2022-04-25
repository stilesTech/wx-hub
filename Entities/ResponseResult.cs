using System;
namespace Entities
{
    public class ResponseResult
    {
        public ErrorCode ErrorCode { get; set; } = ErrorCode.请求成功;

        public String ErrorMsg { get; set; } = ErrorCode.请求成功.ToString();
    }

    public class ResponseResult<T> : ResponseResult
    {
        public T Data { get; set; }
    }

    public enum ErrorCode : int
    {
        系统繁忙 = -1,
        请求成功 = 0,
        参数有误 = 10001,
        校验有误 = 10002,
        参数缺失 = 10003,
        用户不存在 = 11001,
        用户被锁定 = 11002,
        用户没有权限 = 11003,

        其它 = 99999,
    }
}
