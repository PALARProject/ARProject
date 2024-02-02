using UnityEngine;
using System.Collections;
public class Onetouch : MonoBehaviour
{
    public GameObject camera;
    private float Speed = 5f;
    private Vector2 nowPos, prePos;
    private Vector3 movePos;

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                prePos = touch.position - touch.deltaPosition;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                nowPos = touch.position - touch.deltaPosition;
                movePos = (Vector3)(prePos - nowPos) * Time.deltaTime * Speed;
                camera.transform.Translate(movePos);
                prePos = touch.position - touch.deltaPosition;
            }
        }
    }
}