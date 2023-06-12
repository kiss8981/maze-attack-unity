using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public bool limitBounds = false;
    public float left = -5f;
    public float right = 5f;
    public float bottom = -5f;
    public float top = 5f;

    private Vector3 lerpedPosition;

    private Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        if (target != null)
        {
            lerpedPosition = Vector3.Lerp(
                transform.position,
                target.position,
                Time.deltaTime * 10f
            );
            lerpedPosition.z = -10f;
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = lerpedPosition;

            if (limitBounds)
            {
                Vector3 bottomLeft = camera.ScreenToWorldPoint(Vector3.zero);
                Vector3 topRight = camera.ScreenToWorldPoint(
                    new Vector3(camera.pixelWidth, camera.pixelHeight)
                );
                Vector2 screenSize = new Vector2(
                    topRight.x - bottomLeft.x,
                    topRight.y - bottomLeft.y
                );

                Vector3 boundPosition = transform.position;
                if (boundPosition.x > right - (screenSize.x / 2f))
                {
                    boundPosition.x = right - (screenSize.x / 2f);
                }
                if (boundPosition.x < left + (screenSize.x / 2f))
                {
                    boundPosition.x = left + (screenSize.x / 2f);
                }

                if (boundPosition.y > top - (screenSize.y / 2f))
                {
                    boundPosition.y = top - (screenSize.y / 2f);
                }
                if (boundPosition.y < bottom + (screenSize.y / 2f))
                {
                    boundPosition.y = bottom + (screenSize.y / 2f);
                }
                transform.position = boundPosition;
            }
        }
    }
}
