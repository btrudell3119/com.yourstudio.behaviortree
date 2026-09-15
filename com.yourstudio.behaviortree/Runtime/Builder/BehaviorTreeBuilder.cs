using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// Fluent API for assembling a tree in code, e.g.:
    ///
    /// var tree = new BehaviorTreeBuilder()
    ///     .Selector()
    ///         .Sequence()
    ///             .Condition(bb => bb.Get&lt;bool&gt;("canSeePlayer"))
    ///             .Action(bb => Chase(bb))
    ///         .End()
    ///         .Action(bb => Patrol(bb))
    ///     .End()
    ///     .Build();
    /// </summary>
    public class BehaviorTreeBuilder
    {
        private readonly Stack<Composite> _stack = new Stack<Composite>();
        private Node _root;

        private BehaviorTreeBuilder Push(Composite node)
        {
            AttachToParent(node);
            _stack.Push(node);
            return this;
        }

        private void AttachToParent(Node node)
        {
            if (_stack.Count > 0)
                _stack.Peek().Add(node);
            else if (_root == null)
                _root = node;
            else
                throw new InvalidOperationException(
                    "Tree already has a root — wrap multiple top-level nodes in a Selector/Sequence.");
        }

        public BehaviorTreeBuilder Selector() => Push(new Selector());
        public BehaviorTreeBuilder Sequence() => Push(new Sequence());
        public BehaviorTreeBuilder Parallel(ParallelPolicy success = ParallelPolicy.RequireAll,
                                             ParallelPolicy failure = ParallelPolicy.RequireOne)
            => Push(new Parallel(success, failure));

        public BehaviorTreeBuilder Action(Func<Blackboard, NodeStatus> action)
        {
            AttachToParent(new ActionNode(action));
            return this;
        }

        public BehaviorTreeBuilder Condition(Func<Blackboard, bool> condition)
        {
            AttachToParent(new ConditionNode(condition));
            return this;
        }

        public BehaviorTreeBuilder Wait(float seconds)
        {
            AttachToParent(new WaitNode(seconds));
            return this;
        }

        /// <summary>Each Decorate-style call builds a small subtree via a nested builder, then wraps it.</summary>
        public BehaviorTreeBuilder Inverter(Action<BehaviorTreeBuilder> subtree)
            => Decorate(subtree, c => new Inverter(c));

        public BehaviorTreeBuilder Repeater(Action<BehaviorTreeBuilder> subtree, int count = -1, bool repeatOnFailure = false)
            => Decorate(subtree, c => new Repeater(c, count, repeatOnFailure));

        public BehaviorTreeBuilder Cooldown(Action<BehaviorTreeBuilder> subtree, float seconds)
            => Decorate(subtree, c => new Cooldown(c, seconds));

        public BehaviorTreeBuilder ConditionGate(Action<BehaviorTreeBuilder> subtree, Func<Blackboard, bool> condition)
            => Decorate(subtree, c => new ConditionDecorator(c, condition));

        private BehaviorTreeBuilder Decorate(Action<BehaviorTreeBuilder> subtree, Func<Node, Node> wrap)
        {
            var inner = new BehaviorTreeBuilder();
            subtree(inner);
            AttachToParent(wrap(inner.Build()));
            return this;
        }

        public BehaviorTreeBuilder End()
        {
            _stack.Pop();
            return this;
        }

        public Node Build()
        {
            if (_stack.Count > 0)
                throw new InvalidOperationException($"{_stack.Count} composite(s) still open — missing End() call(s).");
            return _root;
        }
    }
}
