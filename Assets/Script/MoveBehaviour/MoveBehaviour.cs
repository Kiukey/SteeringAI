using System;
using UnityEngine;

public class MoveBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody = null;
    [SerializeField] private float speed = 1;

    public Vector3 TargetVelocity => Vector3.forward * speed + Vector3.right * speed * Time.deltaTime;
    
    private void Update()
    {
        if (!rigidbody) return;
        rigidbody.velocity = TargetVelocity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position,0.1f);
    }
}
