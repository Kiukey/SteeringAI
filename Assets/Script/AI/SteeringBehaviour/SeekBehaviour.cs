using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SeekBehaviour", menuName = "Steering Behaviour/Seek")]
public class SeekBehaviour : SteeringBehaviourBase
{
    public override Vector3 Behaviour(Vector3 _velocity)
    {
        return Seek(SteeringManager.Manager.Leader.position, _velocity);
    }
    protected Vector3 Seek(Vector3 _pos,Vector3 _velocity)
    {
        Vector3 _toRet = Vector3.zero;
        SteeringManager _manager = SteeringManager.Manager;
        if (!_manager) return _toRet;
        _toRet = _pos - OwnerPos;
        _toRet = _toRet.normalized * owner.MaxVelocity;
        return _toRet - _velocity;
    }
}
