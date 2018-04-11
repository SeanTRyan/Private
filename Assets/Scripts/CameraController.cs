using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        this.transform.position = target.position + offset;
    }

}
