using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public float speed = 0.001f;


    void FixedUpdate()
    {
        gameObject.transform.Rotate(0f, speed, 0f);
    }
}
