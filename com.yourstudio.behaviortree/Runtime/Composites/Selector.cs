namespace BehaviorTree
{
    /// <summary>Runs children in order until one succeeds or is still running. "Try this, or this, or this."</summary>
    public class Selector : Composite
    {
        private int _current;

        protected override void OnStart(Blackboard blackboard) => _current = 0;

        protected override NodeStatus OnTick(Blackboard blackboard)
        {
            for (; _current < Children.Count; _current++)
            {
                var status = Children[_current].Tick(blackboard);
                if (status != NodeStatus.Failure)
                    return status;
            }
            return NodeStatus.Failure;
        }
    }
}
