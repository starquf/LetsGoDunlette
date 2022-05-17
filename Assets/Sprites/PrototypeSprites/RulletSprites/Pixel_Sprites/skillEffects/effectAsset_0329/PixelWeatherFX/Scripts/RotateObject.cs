using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public bool rotate = true;

    private void Start()
    {
        rotate = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (rotate == true)
        {
            transform.Rotate(Vector3.up * (7 * Time.deltaTime));
        }
    }
}
