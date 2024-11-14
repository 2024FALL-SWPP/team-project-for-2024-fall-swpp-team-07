using UnityEngine;

namespace EasyDestuctibleWall
{
    public class MouseOrbit : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        [SerializeField]
        private float distance = 10f;

        [SerializeField]
        private float xSpeed = 250f;

        [SerializeField]
        private float ySpeed = 120f;

        [SerializeField]
        private float yMinLimit = -20f;

        [SerializeField]
        private float yMaxLimit = 80f;

        private float angleX = 0f;
        private float angleY = 0f;

        void Start()
        {
            Vector3 angles = transform.eulerAngles;
            angleX = angles.y;
            angleY = angles.x;
        }

        // Input is only updated per Update tick.
        void Update()
        {
            if (target == null)
                return;
            // Retrieve the Mouse ScrollWheel axis so we can zoom in and out using the scroll wheel
            float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
            distance -= scrollWheelInput * 220f * Time.deltaTime;

            angleX += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
            angleY -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

            angleY = ClampAngle(angleY, yMinLimit, yMaxLimit);
        }

        // Update the camera position every time the physics engine has updated.
        void FixedUpdate()
        {
            transform.rotation = Quaternion.Euler(angleY, angleX, 0);
            transform.position =
                transform.rotation * new Vector3(0f, 0f, -distance) + target.position;
        }

        static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f)
                angle += 360f;
            else if (angle > 360f)
                angle -= 360f;

            return Mathf.Clamp(angle, min, max);
        }
    }
}
