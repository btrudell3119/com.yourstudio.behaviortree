namespace BehaviorTree
{
    /// <summary>Repeats its child a fixed number of times, or forever if count &lt;= 0. Stops on Failure unless repeatOnFailure is true.</summary>
    public class Repeater : Decorator
    {
        private readonly int _count;
        private readonly bool _repeatOnFailure;
        private int _iterations;

        public Repeater(Node child, int count = -1, bool repeatOnFailure = false) : base(child)
        {
            _count = count;
            _repeatOnFailure = repeatOnFailure;
        }

        protected override void OnStart(Blackboard blackboard) => _iterations = 0;

        protected override NodeStatus OnTick(Blackboard blackboard)
        {
            var status = Child.Tick(blackboard);

            if (status == NodeStatus.Running)
                return NodeStatus.Running;

            if (status == NodeStatus.Failure && !_repeatOnFailure)
                return NodeStatus.Failure;

            _iterations++;
            if (_count > 0 && _iterations >= _count)
                return NodeStatus.Success;

            return NodeStatus.Running; // keep going next tick
        }
    }
}
