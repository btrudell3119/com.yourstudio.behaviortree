namespace BehaviorTree
{
    /// <summary>
    /// Base class for every node in the tree. Pure C# — no MonoBehaviour
    /// dependency — so trees can be built and reused outside any single
    /// scene, prefab, or project.
    /// </summary>
    public abstract class Node
    {
        public NodeStatus Status { get; protected set; } = NodeStatus.Failure;
        private bool _started;

        public NodeStatus Tick(Blackboard blackboard)
        {
            if (!_started)
            {
                OnStart(blackboard);
                _started = true;
            }

            Status = OnTick(blackboard);

            if (Status != NodeStatus.Running)
            {
                OnStop(blackboard);
                _started = false;
            }

            return Status;
        }

        /// <summary>Called once when the node transitions from idle to running.</summary>
        protected virtual void OnStart(Blackboard blackboard) { }

        /// <summary>Called every tick while active. Must return the node's result.</summary>
        protected abstract NodeStatus OnTick(Blackboard blackboard);

        /// <summary>Called once when the node finishes (Success or Failure).</summary>
        protected virtual void OnStop(Blackboard blackboard) { }

        /// <summary>Force-resets a node, e.g. when a parent selector/sequence aborts it mid-run.</summary>
        public virtual void Reset()
        {
            _started = false;
            Status = NodeStatus.Failure;
        }
    }
}
