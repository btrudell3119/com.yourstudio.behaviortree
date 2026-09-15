using UnityEngine;
using BehaviorTree;

/// <summary>
/// Example: a generic patrol -> chase -> attack enemy, built with the tree
/// framework. Only this file is game-specific — everything it calls into
/// (Transform, NavMeshAgent, etc.) can be swapped per project while the
/// Runtime/ folder stays identical across all your games.
/// </summary>
[RequireComponent(typeof(BehaviorTreeRunner))]
public class EnemyAIExample : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private float _chaseRange = 8f;
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float _moveSpeed = 3f;

    private BehaviorTreeRunner _runner;
    private int _patrolIndex;

    private void Awake()
    {
        _runner = GetComponent<BehaviorTreeRunner>();

        _runner.Blackboard.Set("player", _player);
        _runner.Blackboard.Set("self", transform);

        _runner.Root = new BehaviorTreeBuilder()
            .Selector()
                // Attack if close enough
                .Sequence()
                    .Condition(bb => DistanceToPlayer(bb) <= _attackRange)
                    .Action(Attack)
                .End()
                // Chase if player is within range but not close enough to attack
                .Sequence()
                    .Condition(bb => DistanceToPlayer(bb) <= _chaseRange)
                    .Action(ChasePlayer)
                .End()
                // Otherwise patrol
                .Action(Patrol)
            .End()
            .Build();
    }

    private float DistanceToPlayer(Blackboard bb)
    {
        var self = bb.Get<Transform>("self");
        var player = bb.Get<Transform>("player");
        return player == null ? float.MaxValue : Vector3.Distance(self.position, player.position);
    }

    private NodeStatus ChasePlayer(Blackboard bb)
    {
        var player = bb.Get<Transform>("player");
        transform.position = Vector3.MoveTowards(transform.position, player.position, _moveSpeed * Time.deltaTime);
        return NodeStatus.Success;
    }

    private NodeStatus Attack(Blackboard bb)
    {
        Debug.Log($"{name} attacks the player!");
        return NodeStatus.Success;
    }

    private NodeStatus Patrol(Blackboard bb)
    {
        if (_patrolPoints.Length == 0) return NodeStatus.Failure;

        var target = _patrolPoints[_patrolIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, _moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
            _patrolIndex = (_patrolIndex + 1) % _patrolPoints.Length;

        return NodeStatus.Success;
    }
}
