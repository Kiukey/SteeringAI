using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArriveBehaviour", menuName = "Steering Behaviour/Arrive")]
public class ArriveBehaviour : SteeringBehaviourBase
{
    [SerializeField,Range(0,100f)] protected float slowingRadius = 1;
    public override Vector3 Behaviour(Vector3 _velocity)
    {
        return Arrive(SteeringManager.Manager.Leader.position, _velocity);
    }

    protected Vector3 Arrive(Vector3 _target,Vector3 _velocity)
    {
        Vector3 _desiredVelocity = _target - OwnerPos;
        float _dist = _desiredVelocity.magnitude;
        if (_dist < slowingRadius)
        {
            float _divider = (_dist / slowingRadius);
            _desiredVelocity = _desiredVelocity.normalized * (owner.MaxVelocity * _divider);
        }
        else
        {
            _desiredVelocity = _desiredVelocity.normalized * owner.MaxVelocity;
        }
        return _desiredVelocity - _velocity;
    }
}
