using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using V3 = UnityEngine.Vector3;

public class FollowBehaviour : ArriveBehaviour, IManagedItem<int>
{
    private int id = 0;
    [SerializeField,Range(0,100f)] private float behindDistance = 1f;
    [SerializeField,Range(0,1)] private float maxForce = 1;
    [SerializeField] private float maxSpeed = 1;
    
    private Rigidbody leaderRigidBody = null;
    private V3 LeaderPos => AIManager.Manager.Leader.position;
    private V3 LeaderVelocity => leaderRigidBody.velocity;
    
    private void Start()
    {
        Register();
        leaderRigidBody = GetLeaderRigidBody();
    }

    protected override void Update()
    {
        V3 _steering = FollowLeader();

        _steering = Truncate(_steering, maxForce);
        velocity = Truncate(velocity + _steering, maxSpeed);
        transform.position += velocity * Time.deltaTime;

    }
    
    protected override void OnDrawGizmos()
    {
        if (!AIManager.Manager||!AIManager.Manager.Leader)
            return;
        Gizmos.color = Color.cyan;
        V3 _point = GetBehindPoint();
        Gizmos.DrawWireSphere(_point,0.2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_point,slowingRadius);
    }

    private V3 FollowLeader()
    {
        AIManager _manager = AIManager.Manager;
        V3 _toRet = V3.zero;
        if (!_manager) return _toRet;
        _toRet = Arrive(GetBehindPoint());
        _toRet += Avoid();
        return _toRet;
    }
    
    private Rigidbody GetLeaderRigidBody()
    {
        AIManager _manager = AIManager.Manager;
        if (!_manager) return null;
        return _manager.Leader?.GetComponent<Rigidbody>();
    }

    private V3 GetBehindPoint()
    {
        if (!AIManager.Manager?.Leader)
            return V3.zero;
        V3 _invertVelocity = LeaderVelocity * -1;
        _invertVelocity = _invertVelocity.normalized * behindDistance;
        return LeaderPos + _invertVelocity;
    }
    
    private Vector3 Avoid()
    {
        V3 _toRet = V3.zero;
        AIManager _manager = AIManager.Manager;
        if (!_manager) return _toRet;
        int _count = _manager.Count;
        int _nbr = 0;
        for (int i = 0; i < _count; i++)
        {
            FollowBehaviour _other = _manager[i];
            if(!_other) continue;
            float _dist = V3.Distance(_other.transform.position, transform.position);
            if(_other == this || _dist > _manager.RadiusAvoidance) continue;
            _toRet.z += _other.transform.position.z - transform.position.z; 
            _toRet.x += _other.transform.position.x - transform.position.x; 
            _nbr++;
        }

        if (_nbr > 0)
        {
            _toRet.z /= _nbr;
            _toRet.y /= _nbr;
            _toRet *= -1;
        }

        _toRet = _toRet.normalized;
        _toRet *= _manager.maxAvoidence;
        return _toRet - velocity;
    }
    public void Register()
    {
        if (!AIManager.Manager) return;
        id = AIManager.Manager.Count;
        Debug.Log(id);
        AIManager.Manager.Register(id,this);
    }
    public void Unregister()
    {
        if (!AIManager.Manager) return;
        AIManager.Manager.Unregister(id);
    }
}
