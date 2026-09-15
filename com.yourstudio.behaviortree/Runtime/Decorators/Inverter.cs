namespace BehaviorTree
{
    public class Inverter : Decorator
    {
        public Inverter(Node child) : base(child) { }

        protected override NodeStatus OnTick(Blackboard blackboard)
        {
            var status = Child.Tick(blackboard);
            return status switch
            {
                NodeStatus.Success => NodeStatus.Failure,
                NodeStatus.Failure => NodeStatus.Success,
                _ => NodeStatus.Running
            };
        }
    }
}
