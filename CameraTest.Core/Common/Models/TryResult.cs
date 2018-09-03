using System;
namespace CameraTest.Core.Common.Models
{
    public static class TryResult
    {
        public static TryResult<TResult> Create<TResult>(bool operationSucceeded, TResult result)
        {
            return new TryResult<TResult>(operationSucceeded, result);
        }

        public static TryResult<TResult> Unsucceed<TResult>()
        {
            return new TryResult<TResult>(false);
        }
    }

    public class TryResult<TResult>
    {
        public bool OperationSucceeded { get; private set; }
        public TResult Result { get; private set; }

        public TryResult(bool operationSucceeded, TResult result = default(TResult))
        {
            OperationSucceeded = operationSucceeded;
            Result = result;
        }
    }
}
