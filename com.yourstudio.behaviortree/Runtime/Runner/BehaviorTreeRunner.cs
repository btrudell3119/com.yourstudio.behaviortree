using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// Drop on any GameObject to tick a behavior tree. Build the tree in a
    /// subclass's Awake/Start via BehaviorTreeBuilder and assign it to Root.
    /// This is the only MonoBehaviour in the framework — everything else is
    /// plain C# so the AI logic itself has zero scene/project coupling.
    /// </summary>
    public class BehaviorTreeRunner : MonoBehaviour
    {
        public Blackboard Blackboard { get; } = new Blackboard();
        public Node Root { get; set; }

        [Tooltip("0 = tick every frame. Set higher to throttle AI cost for many agents.")]
        [SerializeField] private float _tickInterval = 0f;
        private float _timer;

        protected virtual void Update()
        {
            if (Root == null) return;

            if (_tickInterval <= 0f)
            {
                Root.Tick(Blackboard);
                return;
            }

            _timer += Time.deltaTime;
            if (_timer < _tickInterval) return;
            _timer = 0f;
            Root.Tick(Blackboard);
        }

        public void Restart() => Root?.Reset();
    }
}
