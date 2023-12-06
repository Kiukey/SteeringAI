using System;
using UnityEngine;
using V3 = UnityEngine.Vector3;
public class ArriveBehaviour : MonoBehaviour
{
    [SerializeField] protected float maxVelocity = 0;
    [SerializeField] protected V3 velocity = Vector3.zero;
    [SerializeField,Range(0,100f)] protected float slowingRadius = 1;

    protected virtual void Update()
    {
        velocity = Truncate(velocity + Arrive(AIManager.Manager.Leader.position), maxVelocity);
        // velocity += Arrive(AIManager.Manager.Leader.position);
        transform.position += velocity * Time.deltaTime;
    }

    protected V3 Arrive(V3 _target)
    {
        V3 _desiredVelocity = _target - transform.position;
        float _dist = _desiredVelocity.magnitude;
        if (_dist < slowingRadius)
        {
            float _divider = (_dist / slowingRadius);
            _desiredVelocity = _desiredVelocity.normalized * maxVelocity * _divider;
        }
        else
        {
            _desiredVelocity = _desiredVelocity.normalized * maxVelocity;
        }
        return _desiredVelocity - velocity;
    }

    protected virtual void OnDrawGizmos()
    {
        if (!AIManager.Manager || !AIManager.Manager.Leader)
            return;
        Gizmos.DrawWireSphere(AIManager.Manager.Leader.position,slowingRadius);
    }

    protected V3 Truncate(V3 _toClamp, float _maxValue)
    {
        float _i = _maxValue / _toClamp.magnitude;
        _i = _i < 1 ? _i : 1;
        return _toClamp * _i;
    }
}
