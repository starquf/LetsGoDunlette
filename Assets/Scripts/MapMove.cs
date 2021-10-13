using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMove : MonoBehaviour
{
    public float moveSpeed;

    private void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }
}
