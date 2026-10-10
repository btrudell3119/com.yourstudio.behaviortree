using System;

namespace BehaviorTree
{
    /// <summary>Only ticks its child while `condition` is true; otherwise fails immediately (a "guard").</summary>
    public class ConditionDecorator : Decorator
    {
        private readonly Func<Blackboard, bool> _condition;

        public ConditionDecorator(Node child, Func<Blackboard, bool> condition) : base(child)
            => _condition = condition;

        protected override NodeStatus OnTick(Blackboard blackboard)
            => _condition(blackboard) ? Child.Tick(blackboard) : NodeStatus.Failure;
    }
}
