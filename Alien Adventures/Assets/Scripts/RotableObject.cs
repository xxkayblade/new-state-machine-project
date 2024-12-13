using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotableObject : MonoBehaviour
{
    public float rotationSpeed = 0.1f;

    private void Update()
    {
        transform.Rotate(0, 0, rotationSpeed);
    }
}
