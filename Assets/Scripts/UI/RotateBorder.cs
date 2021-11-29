using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBorder : MonoBehaviour
{
    [SerializeField] private float slowSpeed = 1.5f;
    [SerializeField] private float fastSpeed = 5f;

    private float speed;

    private void Start()
    {
        speed = slowSpeed;
    }

    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, speed * Time.deltaTime));
    }

    public void SetSpeed(bool isFast)
    {
        speed = isFast ? fastSpeed : slowSpeed;
    }
}
