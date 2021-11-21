using UnityEngine;

namespace Project.Movement
{
    public class MoveAroundObject : MonoBehaviour
    {
        [SerializeField]
        private float _mouseSensitivity = 3.0f;

        private float _rotationX;
        private float _rotationY;

        // Update is called once per frame
        private void Update()
        {
            var mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
            var mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

            _rotationY += mouseX;
            _rotationX += mouseY;

            _rotationX = Mathf.Clamp(_rotationX, -40, 40);

            transform.localEulerAngles = new Vector3(0, _rotationY, 0);
            // Debug.Log($"Wes - mouseX = {mouseX}");
        }
    }
}