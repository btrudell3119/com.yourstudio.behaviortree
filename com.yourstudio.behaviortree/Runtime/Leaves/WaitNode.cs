using UnityEngine;

namespace BehaviorTree
{
    public class WaitNode : Node
    {
        private readonly float _seconds;
        private float _endTime;

        public WaitNode(float seconds) => _seconds = seconds;

        protected override void OnStart(Blackboard blackboard) => _endTime = Time.time + _seconds;

        protected override NodeStatus OnTick(Blackboard blackboard)
            => Time.time >= _endTime ? NodeStatus.Success : NodeStatus.Running;
    }
}
