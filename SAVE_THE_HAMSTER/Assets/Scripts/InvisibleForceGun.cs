using System.Collections;
using UnityEngine;

public class InvisibleForceGun : MonoBehaviour
{
    [SerializeField]
    private float forceMultiplier = 900f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            // Convert the position of the mouse cursor from screen space to world space
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 50.0f))
            {
                Rigidbody hitObject = hit.rigidbody;
                if (hit.rigidbody == null)
                    return;

                Vector3 direction = hitObject.transform.position - transform.position;
                hitObject.AddForceAtPosition(direction.normalized * forceMultiplier, hit.point);
            }
        }
    }
}
