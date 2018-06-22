namespace HttpExecutor
{
    public interface IExecutionStatus
    {
        int TotalExecutions { get; }

        int SuccessCount { get; }

        int FailureCount { get; }
    }
}
