using Unity.VisualScripting;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 6f;     // drag / damping
    public Vector3 offset;             // camera height + angle
    public float verticalScreenBias = -3f;
    private float fixedY;               // stored camera Y

    void Start()
    {
        fixedY = transform.position.y;  // lock camera height
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Follow only X and Z of player. Y stays fixed.
        Vector3 targetPos = new Vector3(
            target.position.x + offset.x,
            fixedY + offset.y,
            target.position.z + offset.z + verticalScreenBias
        );

        // Smooth follow (drag)
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);

        // Keep camera rotation unchanged
    }
}

    


