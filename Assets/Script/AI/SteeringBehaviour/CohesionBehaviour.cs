using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cohesion Behaviour", menuName = "Steering Behaviour/Cohesion")]
public class CohesionBehaviour : SteeringBehaviourBase
{
    [SerializeField] float steeringForce = 1.0f;

    public override Vector3 Behaviour(Vector3 _velocity)
    {
        Vector3 _averagePos = Vector3.zero;
        int _neighbourCount = 0;
        int _totalNeighbourCount = owner.Neighbours.Count;
        for (int i = 0; i < _totalNeighbourCount; i++)
        {
            GameObject _neighbour = owner.Neighbours[i];
            if (!IsInRange(_neighbour.transform.position, 1)) continue;
            _averagePos += _neighbour.transform.position;
            _neighbourCount++;
        }
        if (_neighbourCount > 0)
            _averagePos = _averagePos / _neighbourCount;
        Vector3 _result = _neighbourCount > 0 ? (_averagePos - OwnerPos) * steeringForce :Vector3.zero ;
        return _result;
    }
    public override void DebugState()
    {
        base.DebugState();
        //Vector3 _averagePos = Vector3.zero;
        //SteeringManager _manager = SteeringManager.Manager;
        //if (_manager == null)
        //    return;
        //int _neighbourCount = 0;
        ////for (int i = 0; i < _manager.Count; i++)
        ////{
        ////    if (Vector3.Distance(OwnerPos, _manager[i].transform.position) > range) continue;
        ////    _averagePos += _manager[i].transform.position;
        ////    _neighbourCount++;
        ////}

        //Gizmos.DrawCube(_averagePos / _neighbourCount, Vector3.one);
    }
}
