using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public abstract class StateBase
{
    private FSMBase owner = null;
    private List<TransitionBase> transitions = new List<TransitionBase>();

    public int TransitionNumber => transitions.Count;

    protected void CreateTransition<T>() where T : new()
    {
        TransitionBase _transition = new T() as TransitionBase;
        if (!_transition) return;
        _transition.InitTransition(this);
    }
    
    #region Behaviors
    private void CheckTransitions()
    {
        foreach (TransitionBase _transition in transitions)
        {
            if (!_transition) continue;
            if (_transition.CheckTransition())
            {
                _transition.OnTransitionComplete();
                return;
            }
        }
    }
    public virtual void InitState(FSMBase _owner)
    {
        owner = _owner;
    }
    public virtual void DestroyState()
    {
    }
    public virtual void UpdateState(float _deltaTime)
    {
        CheckTransitions();
    }
    #endregion
    #region operators

    public static bool operator !(StateBase _me) => _me.Equals(null);
    
    #endregion
}
