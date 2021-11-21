using UnityEngine;

namespace Project.Movement
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;

        [SerializeField]
        private float _smoothSpeed = 0.125f;

        [SerializeField]
        private Vector3 _offset;

        private void FixedUpdate()
        {
            var desiredPosition = _target.position + _offset;
            var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);

            transform.position = smoothedPosition;

            // transform.position = _target.position + _offset;
        }
    }
}