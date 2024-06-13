using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Align Behaviour", menuName = "Steering Behaviour/Align")]
public class AlignBehaviour : SteeringBehaviourBase
{
    public override Vector3 Behaviour(Vector3 _velocity)
    {
        return Alignement(_velocity);   
    }

    Vector3 Alignement(Vector3 _velocity)
    {
        float _adjustmentForce = 1f;
        Vector3 _toRet = Vector3.zero;
        int _neighbourNumber = 0;
        Vector3 _averageVelocity = Vector3.zero;
        List<GameObject> _list = owner.Neighbours;

        foreach (GameObject _neighbour in _list)
        {
            SteeringComponent _component = _neighbour.GetComponent<SteeringComponent>();
            if (!_component||!IsInRange(_component.transform.position, 2)) continue;

            _neighbourNumber++;
            _averageVelocity += _component.CurrentVelocity;
        }
        if(_neighbourNumber > 0)
        {
            _averageVelocity /= _neighbourNumber;
            _toRet += (_averageVelocity - _velocity) * _adjustmentForce;
        }

        Debug.Log(_toRet);
        return _toRet;
    }

}
