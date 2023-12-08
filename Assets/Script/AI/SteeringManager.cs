using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringManager : SingletonTemplate<SteeringManager>, IManager<int, SteeringComponent>
{
    public SteeringComponent this[int _index] => Get(_index);
    private Dictionary<int, SteeringComponent> managedItems = new Dictionary<int, SteeringComponent>();
    [SerializeField,Range(0,10000)] private float radiusAvoidance = 2;
    [SerializeField] public float maxAvoidence = 1;
    [SerializeField] private Transform leader = null;


    public Transform Leader => leader;
    public float RadiusAvoidance => radiusAvoidance;
    public int Count => managedItems.Count;
    
    public void Register(int _key, SteeringComponent _value)
    {
        if (Exist(_key))
            return;
        managedItems.Add(_key, _value);
    }
    public void Unregister(int _key)
    {
        if (!Exist(_key))
            return;
        managedItems.Remove(_key);
    }
    public bool Exist(int _key)
    {
        return managedItems.ContainsKey(_key);
    }
    public SteeringComponent Get(int _key)
    {
        if (!Exist(_key)) return null;
        return managedItems[_key];
    }

}
