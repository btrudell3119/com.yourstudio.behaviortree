using System.Collections.Generic;

namespace BehaviorTree
{
    public abstract class Composite : Node
    {
        protected readonly List<Node> Children = new List<Node>();

        public Composite Add(Node child)
        {
            Children.Add(child);
            return this;
        }

        public override void Reset()
        {
            base.Reset();
            foreach (var child in Children)
                child.Reset();
        }
    }
}
