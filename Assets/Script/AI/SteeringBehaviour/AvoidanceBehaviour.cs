using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvoidanceBehaviour", menuName = "Steering Behaviour/Avoidance")]
public class AvoidanceBehaviour : SteeringBehaviourBase
{
    public override Vector3 Behaviour(Vector3 _velocity)
    {
        return Avoid(_velocity);
    }

    private Vector3 Avoid(Vector3 _velocity)
    {
        Vector3 _toRet = Vector3.zero;
        SteeringManager _manager = SteeringManager.Manager;
        if (!_manager) return _toRet;
        int _count = _manager.Count;
        int _nbr = 0;
        for (int i = 0; i < _count; i++)
        {
            SteeringComponent _other = _manager[i];
            if(!_other || _other == owner ) continue;
            float _dist = Vector3.Distance(_other.transform.position, OwnerPos);
            if(_dist > _manager.RadiusAvoidance) continue;
            // _toRet.z += _other.transform.position.z - transform.position.z; 
            // _toRet.x += _other.transform.position.x - transform.position.x; 
            _toRet += _other.transform.position - OwnerPos;
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
        return _toRet - _velocity;
    }
}
