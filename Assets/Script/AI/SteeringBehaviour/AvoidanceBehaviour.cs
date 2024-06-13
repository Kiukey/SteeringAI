using UnityEngine;

[CreateAssetMenu(fileName = "AvoidanceBehaviour", menuName = "Steering Behaviour/Avoidance")]
public class AvoidanceBehaviour : SteeringBehaviourBase
{
    [SerializeField] float maxAvoidance = 2;

    public override Vector3 Behaviour(Vector3 _velocity)
    {
        return Avoid(_velocity);
    }

    private Vector3 Avoid(Vector3 _velocity)
    {
        Vector3 _toRet = Vector3.zero;
        int _count = owner.Neighbours.Count;
        int _nbr = 0;
        for (int i = 0; i < _count; i++)
        {
            GameObject _other = owner.Neighbours[i];
            if (!IsInRange(_other.transform.position, 3)) continue;
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
        _toRet *= maxAvoidance;
        return _toRet;
    }
}
