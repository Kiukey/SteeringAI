using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityToolBox.GizmosTools;

[RequireComponent(typeof(SphereCollider),typeof(Rigidbody))]
public class SteeringComponent : MonoBehaviour, IManagedItem<int>
{
    private int id = 0;
    private SphereCollider collider = null;
    [SerializeField] private List<SteeringBehaviourBase> steeringsBehaviours = new List<SteeringBehaviourBase>();
    [SerializeField] private Vector3 currentVelocity = Vector3.zero;
    [SerializeField/*,Range(0,1)*/] private float maxSteeringForce = 0;
    [SerializeField] private float maxSpeed = 0;
    [SerializeField] private float maxVelocity = 10;
    [SerializeField,Category("Debug")] List<Color> circleColors = new List<Color>();
    [SerializeField] private bool useDebug = true;

    private List<GameObject> neighbours = new List<GameObject>();
    public List<GameObject> Neighbours => neighbours;
    public float ColliderRadius => collider.radius;
    public float MaxVelocity => maxVelocity;
    public float MaxSpeed => maxSpeed;
    public float MaxSteeringForce => maxSteeringForce;
    public Vector3 CurrentVelocity => currentVelocity;

    private void Awake()
    {
        collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;
    }
    private void Start()
    {
        Register();
        InitSteeringBehaviours();
    }

    private void Update()
    {
        Vector3 _steering = GetSteeringForces();
        //_steering = Truncate(_steering, maxSteeringForce);
        //velocity = Truncate(velocity + _steering, maxSpeed);
        //transform.position += velocity * Time.deltaTime;
        
        //targetVelocity = AvoidNeighbours();
        //targetVelocity += Cohesion();
        //currentVelocity = 
        currentVelocity = Vector3.MoveTowards(Vector3.zero, _steering, maxSteeringForce * Time.deltaTime);
        transform.position += currentVelocity;
    }

    private void OnDrawGizmos()
    {
        if (!collider||!useDebug) return;
        foreach (SteeringBehaviourBase _behaviour in steeringsBehaviours)
        {
            if(_behaviour == null) continue;
            _behaviour.DebugState();
        }
        for (int i = 1; i <= 3; i++)
        {
            GizmosUtils.DrawWireCircle(transform.position, ColliderRadius / i, circleColors[i - 1], 20);
        }
    }

    private Vector3 GetSteeringForces()
    {
        Vector3 _toRet = Vector3.zero;
        foreach (SteeringBehaviourBase _behaviour in steeringsBehaviours)
        {
            _toRet += _behaviour.Behaviour(currentVelocity);
        }
        return _toRet;
    }
    void InitSteeringBehaviours()
    {
        int _count = steeringsBehaviours.Count;
        for (int _i = 0; _i < _count; _i++)
        {
            SteeringBehaviourBase _behaviour = steeringsBehaviours[_i];
            if(!_behaviour) continue;
            steeringsBehaviours[_i] = Instantiate(_behaviour);
            steeringsBehaviours[_i].Initialize(this);
        }
    }
    private Vector3 Truncate(Vector3 _toClamp, float _maxValue)
    {
        float _i = _maxValue / _toClamp.magnitude;
        _i = _i < 1 ? _i : 1;
        return _toClamp * _i;
    }

    public void Register()
    {
        if (!SteeringManager.Manager) return;
        id = SteeringManager.Manager.Count;
        Debug.Log(id);
        SteeringManager.Manager.Register(id,this);
    }
    public void Unregister()
    {
        if (!AIManager.Manager) return;
        SteeringManager.Manager.Unregister(id);
    }

    #region trigger
    private void OnTriggerEnter(Collider _other)
    {
        neighbours.Add(_other.gameObject);
    }
    private void OnTriggerExit(Collider _other)
    {
        neighbours.Remove(_other.gameObject);
    }
    #endregion
    #region
   
    #endregion
}
