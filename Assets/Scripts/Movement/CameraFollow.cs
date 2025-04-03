using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float camSpeed = 1.5f;
    public Transform target;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = new Vector3(target.position.x, target.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, pos, camSpeed);
    }
}
