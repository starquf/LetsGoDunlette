using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class rouletteCollision
{
    public rouletteProperty roulette;
    public int propertyIdx = 0;
    public Quaternion middleAngle = Quaternion.identity;
    public bool isCollision = false;
}

public class roulettepiece : MonoBehaviour
{
    public int size = 0;
    public bool isSetting = false;

    public Transform target;

    private float angleRange = 20f;
    public float distance = 250f;
    private Vector3 dir;
    public List<rouletteCollision> rouletteCollisions;

    Color _blue = new Color(0f, 0f, 1f, 0.2f);
    Color _red = new Color(1f, 0f, 0f, 0.2f);

    Vector3 direction;

    float dotValue = 0f;


    void Update()
    {
        if (!isSetting)
            return;
        for (int i = 0; i < rouletteCollisions.Count; i++)
        {
            dir = rouletteCollisions[i].middleAngle * transform.up;
            dotValue = Mathf.Cos(Mathf.Deg2Rad * (angleRange / 2));
            direction = target.position - transform.position;
            if (direction.magnitude < distance)
            {
                if (Vector3.Dot(direction.normalized, dir) > dotValue)
                    rouletteCollisions[i].isCollision = true;
                else
                    rouletteCollisions[i].isCollision = false;
            }
            else
                rouletteCollisions[i].isCollision = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (!isSetting)
            return;
        for (int i = 0; i < rouletteCollisions.Count; i++)
        {
            Handles.color = rouletteCollisions[i].isCollision ? _red : _blue;
            Handles.DrawSolidArc(transform.position, Vector3.forward, rouletteCollisions[i].middleAngle * transform.up, angleRange / 2, distance);
            Handles.DrawSolidArc(transform.position, Vector3.forward, rouletteCollisions[i].middleAngle * transform.up, -angleRange / 2, distance);
        }

    }
}