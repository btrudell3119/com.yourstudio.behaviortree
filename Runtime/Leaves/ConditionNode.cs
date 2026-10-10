using System;

namespace BehaviorTree
{
    public class ConditionNode : Node
    {
        private readonly Func<Blackboard, bool> _condition;

        public ConditionNode(Func<Blackboard, bool> condition) => _condition = condition;

        protected override NodeStatus OnTick(Blackboard blackboard)
            => _condition(blackboard) ? NodeStatus.Success : NodeStatus.Failure;
    }
}
