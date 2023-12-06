using UnityEngine;
using V3 = UnityEngine.Vector3;
public class SeekingBehaviour : MonoBehaviour
{
    [SerializeField] protected float maxVelocity = 0;
    [SerializeField] protected V3 velocity = Vector3.zero;
    [SerializeField,Range(0,1)] private float maxForce = 1;
    [SerializeField] private float maxSpeed = 1;
    public float CurrentDist => V3.Distance(AIManager.Manager.Leader.position, transform.position);
    
    protected virtual void Update()
    {
        V3 _steering = Truncate(Seek(AIManager.Manager.Leader),maxForce);
        velocity = Truncate(velocity + _steering, maxSpeed);
        transform.position += velocity * Time.deltaTime;
    }

    protected V3 Seek(Transform _target)
    {
        V3 _toRet = V3.zero;
        AIManager _manager = AIManager.Manager;
        if (!_manager || !_target) return _toRet;

        _toRet.z = _target.position.z - transform.position.z; 
        _toRet.x = _target.position.x - transform.position.x;
        _toRet = _toRet.normalized * maxVelocity;
        return _toRet - velocity;
    }
    
    protected V3 Truncate(V3 _toClamp, float _maxValue)
    {
        float _i = _maxValue / _toClamp.magnitude;
        _i = _i < 1 ? _i : 1;
        return _toClamp * _i;
    }
}
