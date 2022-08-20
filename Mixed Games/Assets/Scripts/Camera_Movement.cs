using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    [SerializeField] private Transform _target;

    Vector3 _distanceToTarget;

    private void Awake()
    {
        _distanceToTarget = transform.position - _target.position;
        
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position + _distanceToTarget, 0.125f);
    }
}
