using System;
using UnityEngine;

namespace Project.Movement
{
    public class CameraMovement : MonoBehaviour
    {
        private Camera _camera;

        private Vector3 _dragOrigin;

        private void Update()
        {
            PanCamera();
        }

        private void PanCamera()
        {
            // Save position of mouse in world space when drag starts (first time clicked)

            if (Input.GetMouseButtonDown(0))
            {
                _dragOrigin = _camera.ScreenToWorldPoint(Input.mousePosition);
            }

            // Calculate distance between drag origin and new position if it is still held down
            if (Input.GetMouseButton(0))
            {
                var newPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                var difference = _dragOrigin - newPosition;

                Debug.Log($"Wes - _dragOrigin = {_dragOrigin}, newPosition = {newPosition} --> difference = {difference}");

                // Move the camera by that distance
                _camera.transform.position += difference;
            }
        }
    }
}