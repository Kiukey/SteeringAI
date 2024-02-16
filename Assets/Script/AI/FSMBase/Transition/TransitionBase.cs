using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TransitionBase
{
    protected StateBase owner = null;
    public static bool operator !(TransitionBase _me) => _me == null;

    public virtual void InitTransition(StateBase _owner)
    {
        owner = _owner;
    }
    public virtual bool CheckTransition() => false;

    public virtual void OnTransitionComplete()
    {
    }
}
