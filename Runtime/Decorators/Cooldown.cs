using UnityEngine;

namespace BehaviorTree
{
    /// <summary>Blocks its child from re-running until `seconds` have passed since it last finished.</summary>
    public class Cooldown : Decorator
    {
        private readonly float _seconds;
        private float _readyAt;

        public Cooldown(Node child, float seconds) : base(child) => _seconds = seconds;

        protected override NodeStatus OnTick(Blackboard blackboard)
        {
            if (Time.time < _readyAt)
                return NodeStatus.Failure;

            var status = Child.Tick(blackboard);
            if (status != NodeStatus.Running)
                _readyAt = Time.time + _seconds;

            return status;
        }
    }
}
