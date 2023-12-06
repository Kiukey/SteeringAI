using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonTemplate<Type>: MonoBehaviour where Type : class
{
    private static Type manager = null;
    public static Type Manager => manager;

    protected virtual void Awake()
    {
        if (manager != null)
        {
            Debug.LogError("Manager Already Existing");
            Destroy(this);
            return;
        }
        manager = this as Type;
    }
}
