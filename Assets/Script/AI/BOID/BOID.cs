using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
using UnityToolBox.GizmosTools;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class BOID : MonoBehaviour
{
    [SerializeField] List<GameObject> neighbours = new List<GameObject>();
    [SerializeField] float maxAvoidance = 2;
    [SerializeField] float cohesionForce = 1;
    SphereCollider boidCollider = null;
    [SerializeField] Vector3 currentVelocity = Vector3.zero;
    [SerializeField] List<Color> circleColors = new List<Color>();
    [SerializeField] float acceleration = 1;
    [SerializeField] bool useDebug = false;
    Vector3 targetVelocity = Vector3.zero;
    public Vector3 OwnerPos => transform.position;
    

    private void Start()
    {
        boidCollider = GetComponent<SphereCollider>();
    }
    #region Updates
    private void LateUpdate()
    {
        targetVelocity = AvoidNeighbours();
        targetVelocity += Cohesion();
        transform.position += Vector3.MoveTowards(currentVelocity,targetVelocity, acceleration * Time.deltaTime); 
    }
    #endregion
    #region Triggers
    private void OnTriggerEnter(Collider _other)
    {
        Debug.Log("adding neighbour");
        neighbours.Add(_other.gameObject);
    }
    private void OnTriggerExit(Collider _other)
    {
        neighbours.Remove(_other.gameObject);
    }
    #endregion
    #region Behaviour
    private Vector3 AvoidNeighbours()
    {
        Vector3 _toRet = Vector3.zero;
        
        int _count = neighbours.Count;
        int _nbr = 0;
        for (int i = 0; i < _count; i++)
        {
            GameObject _other = neighbours[i];
            //float _distance = Vector3.Distance(OwnerPos, _other.transform.position);
            //Debug.Log(_distance);
            if (!IsInRange(_other.transform.position,3)) continue;
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
    private Vector3 Cohesion()
    {
        Vector3 _averagePos = Vector3.zero;
        int _neighbourCount = 0;
        for (int i = 0; i < neighbours.Count; i++)
        {

            if (!IsInRange(neighbours[i].transform.position, 1)) continue;
            
            _averagePos += neighbours[i].transform.position;
            _neighbourCount++;
        }
        if (_neighbourCount > 0)
            _averagePos = _averagePos / _neighbourCount;
        Vector3 _result = (_averagePos - OwnerPos) * cohesionForce;
        return _result;
    }
    #endregion

    private bool IsInRange(Vector3 _other, int _radiusIndex)
    {
        float _distance = Vector3.Distance(OwnerPos, _other);
        return _distance < (boidCollider.radius / _radiusIndex) * 2;
    }

    private void OnDrawGizmos()
    {
        if(!boidCollider || !useDebug) return;
        for (int i = 1; i <= 3; i++)
        {
            GizmosUtils.DrawWireCircle(transform.position, boidCollider.radius / i, circleColors[i-1], 20);
        }
    }
}
