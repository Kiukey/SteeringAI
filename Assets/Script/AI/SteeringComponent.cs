using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class SteeringComponent : MonoBehaviour, IManagedItem<int>
{
    private int id = 0;
    [SerializeField] private List<SteeringBehaviourBase> steeringsBehaviours = new List<SteeringBehaviourBase>();
    [SerializeField] private Vector3 velocity = Vector3.zero;
    [SerializeField,Range(0,1)] private float maxSteeringForce = 0;
    [SerializeField] private float maxSpeed = 0;
    [SerializeField] private float maxVelocity = 10;

    public float MaxVelocity => maxVelocity;
    public float MaxSpeed => maxSpeed;
    public float MaxSteeringForce => maxSteeringForce;
    
    private void Start()
    {
        Register();
        InitSteeringBehaviours();
    }

    private void Update()
    {
        Vector3 _steering = GetSteeringForces();
        _steering = Truncate(_steering, maxSteeringForce);
        velocity = Truncate(velocity + _steering, maxSpeed);
        transform.position += velocity * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        foreach (SteeringBehaviourBase _behaviour in steeringsBehaviours)
        {
            if(_behaviour == null) continue;
            _behaviour.DebugState();
        }
    }

    private Vector3 GetSteeringForces()
    {
        Vector3 _toRet = Vector3.zero;
        foreach (SteeringBehaviourBase _behaviour in steeringsBehaviours)
        {
            _toRet += _behaviour.Behaviour(velocity);
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
}
