namespace BehaviorTree
{
    /// <summary>Runs children in order until one fails or is still running. "Do this, then this, then this."</summary>
    public class Sequence : Composite
    {
        private int _current;

        protected override void OnStart(Blackboard blackboard) => _current = 0;

        protected override NodeStatus OnTick(Blackboard blackboard)
        {
            for (; _current < Children.Count; _current++)
            {
                var status = Children[_current].Tick(blackboard);
                if (status != NodeStatus.Success)
                    return status;
            }
            return NodeStatus.Success;
        }
    }
}
