using UnityEngine;

public class RotateTornado : MonoBehaviour
{

    public float speed = 30f;
    private Vector3 pivot = new Vector3(0, 50, 0);

    private void Update()
    {
        transform.RotateAround(pivot, Vector3.up, speed * Time.deltaTime);
    }
}
