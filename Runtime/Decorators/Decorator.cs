namespace BehaviorTree
{
    public abstract class Decorator : Node
    {
        protected readonly Node Child;

        protected Decorator(Node child) => Child = child;

        public override void Reset()
        {
            base.Reset();
            Child.Reset();
        }
    }
}
