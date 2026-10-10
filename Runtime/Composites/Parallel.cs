namespace BehaviorTree
{
    public enum ParallelPolicy { RequireOne, RequireAll }

    /// <summary>Ticks every child every frame — e.g. "move toward target" AND "watch for player" at once.</summary>
    public class Parallel : Composite
    {
        private readonly ParallelPolicy _successPolicy;
        private readonly ParallelPolicy _failurePolicy;

        public Parallel(ParallelPolicy successPolicy = ParallelPolicy.RequireAll,
                         ParallelPolicy failurePolicy = ParallelPolicy.RequireOne)
        {
            _successPolicy = successPolicy;
            _failurePolicy = failurePolicy;
        }

        protected override NodeStatus OnTick(Blackboard blackboard)
        {
            int successCount = 0, failureCount = 0;

            foreach (var child in Children)
            {
                var status = child.Tick(blackboard);
                if (status == NodeStatus.Success) successCount++;
                else if (status == NodeStatus.Failure) failureCount++;
            }

            if (_failurePolicy == ParallelPolicy.RequireOne && failureCount > 0) return NodeStatus.Failure;
            if (_failurePolicy == ParallelPolicy.RequireAll && failureCount == Children.Count) return NodeStatus.Failure;
            if (_successPolicy == ParallelPolicy.RequireOne && successCount > 0) return NodeStatus.Success;
            if (_successPolicy == ParallelPolicy.RequireAll && successCount == Children.Count) return NodeStatus.Success;

            return NodeStatus.Running;
        }
    }
}
