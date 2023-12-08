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
}
