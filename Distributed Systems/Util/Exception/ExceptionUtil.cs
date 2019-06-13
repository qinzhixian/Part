namespace Util.Exception
{
    /// <summary>
    /// 异常类
    /// </summary>
    public class ExceptionUtil : System.Exception
    {
        /// <summary>
        /// 异常消息
        /// </summary>
        public override string Message { get; }

        /// <summary>
        /// 异常对象
        /// </summary>
        public System.Exception InnerException;

        public ExceptionType ExceptionType;

        /// <summary>
        /// 创建一个新的异常
        /// </summary>
        /// <param name="message">异常信息</param>
        public ExceptionUtil(string message, ExceptionType exceptionType = ExceptionType.UnKnown)
        {
            Message = message;
            ExceptionType = exceptionType;
        }

        /// <summary>
        /// 创建一个新的异常
        /// </summary>
        /// <param name="message">异常信息</param>
        /// <param name="exception">异常</param>
        public ExceptionUtil(string message, System.Exception exception, ExceptionType exceptionType = ExceptionType.UnKnown)
        {
            Message = message;
            InnerException = exception;
            ExceptionType = exceptionType;
        }
    }
}
