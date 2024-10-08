using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField]
    private bool x, y, z;
    [SerializeField]
    private Transform target;

    private void Update()
    {
        if (!target) return;

        transform.position = new Vector3(
            (x ? target.position.x : transform.position.x),
            (y ? target.position.x : transform.position.y),
            (z ? target.position.x : transform.position.z));
    }
}
