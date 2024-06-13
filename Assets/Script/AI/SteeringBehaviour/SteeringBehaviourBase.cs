using UnityEngine;

public abstract class SteeringBehaviourBase : ScriptableObject
{
    protected SteeringComponent owner = null;
    public Vector3 OwnerPos => owner.transform.position;
    public virtual void Initialize(SteeringComponent _owner)
    {
        owner = _owner;
    }
    public abstract Vector3 Behaviour(Vector3 _velocity);

    public virtual void DebugState()
    {
        
    }

    protected bool IsInRange(Vector3 _other, int _radiusIndex)
    {
        float _distance = Vector3.Distance(OwnerPos, _other);
        return _distance < (owner.ColliderRadius / _radiusIndex) * 2;
    }
}
