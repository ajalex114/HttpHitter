namespace HttpExecutor.Impl
{
    class ExecutionStatus : IExecutionStatus
    {
        public int TotalExecutions { get; set; }

        public int SuccessCount { get; set; }

        public int FailureCount { get; set; }
    }
}
