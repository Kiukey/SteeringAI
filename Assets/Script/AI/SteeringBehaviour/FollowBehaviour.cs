using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

[CreateAssetMenu(fileName = "FollowBehaviour", menuName = "Steering Behaviour/Follow")]
public class FollowBehaviour : ArriveBehaviour
{
    [SerializeField,Range(0,100f)] private float behindDistance = 1f;
    private Rigidbody leaderRigidBody = null;
    private Vector3 LeaderVelocity => leaderRigidBody.velocity;
    private Vector3 LeaderPos => SteeringManager.Manager.Leader.position;

    public override Vector3 Behaviour(Vector3 _velocity)
    {
        return FollowLeader(GetBehindPoint(), _velocity);
    }

    private Vector3 FollowLeader(Vector3 _position, Vector3 _velocity)
    {
        SteeringManager _manager = SteeringManager.Manager;
        Vector3 _toRet = Vector3.zero;
        if (!_manager) return _toRet;
        _toRet += Arrive(_position,_velocity);
        return _toRet;
    }

    public override void Initialize(SteeringComponent _owner)
    {
        base.Initialize(_owner);
        leaderRigidBody = GetLeaderRigidBody();
    }
    private Rigidbody GetLeaderRigidBody()
    {
        AIManager _manager = AIManager.Manager;
        if (!_manager) return null;
        return _manager.Leader?.GetComponent<Rigidbody>();
    }

    private Vector3 GetBehindPoint()
    {
        if (!SteeringManager.Manager?.Leader || !leaderRigidBody)
            return Vector3.zero;
        Vector3 _invertVelocity = LeaderVelocity * -1;
        _invertVelocity = _invertVelocity.normalized * behindDistance;
        return LeaderPos + _invertVelocity;
    }
}
