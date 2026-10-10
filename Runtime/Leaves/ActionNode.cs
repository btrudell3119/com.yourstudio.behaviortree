using System;

namespace BehaviorTree
{
    /// <summary>Wraps a plain function as a leaf node — the fastest way to author game-specific behavior without subclassing.</summary>
    public class ActionNode : Node
    {
        private readonly Func<Blackboard, NodeStatus> _action;

        public ActionNode(Func<Blackboard, NodeStatus> action) => _action = action;

        protected override NodeStatus OnTick(Blackboard blackboard) => _action(blackboard);
    }
}
