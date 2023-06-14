using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField, Header("추적할 오브젝트")]
    private Transform target;

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
        }
    }
}
