using System;
using DataStruct.Enum;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Movement
{
    public enum CameraZoomMode
    {
        In,
        Out
    }

    public class CameraMovement : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera = null;

        [SerializeField]
        private float _zoomStep = 1;

        [SerializeField]
        private int _minCameraSize = 1;

        [SerializeField]
        private int _maxCameraSize = 11;

        [SerializeField]
        private Button _zoomInButton = null;

        [SerializeField]
        private Button _zoomOutButton = null;

        [SerializeField]
        private SpriteRenderer _mapRenderer = null;

        private float _mapMinX, _mapMaxX, _mapMinY, _mapMaxY;

        public bool MouseScrollForward => Input.GetAxis("Mouse ScrollWheel") > 0f;

        public bool MouseScrollBackward => Input.GetAxis("Mouse ScrollWheel") < 0f;

        private Vector3 _dragOrigin;
        
        private void Awake()
        {
            SetCameraBound();
        }

        private void Update()
        {
            PanCamera();
        }
        
        private void SetCameraBound()
        {
            _mapMinX = _mapRenderer.transform.position.x - _mapRenderer.bounds.size.x / 2f;
            _mapMaxX = _mapRenderer.transform.position.x + _mapRenderer.bounds.size.x / 2f;

            _mapMinY = _mapRenderer.transform.position.y - _mapRenderer.bounds.size.y / 2f;
            _mapMaxY = _mapRenderer.transform.position.y + _mapRenderer.bounds.size.y / 2f;
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

                // Debug.Log($"Wes - _dragOrigin = {_dragOrigin}, newPosition = {newPosition} --> difference = {difference}");

                // Move the camera by that distance
                // _camera.transform.position += difference;

                _camera.transform.position = ClampCamera(_camera.transform.position + difference);
            }

            if (MouseScrollForward)
            {
                ZoomCamera(CameraZoomMode.In);
            }

            if (MouseScrollBackward)
            {
                ZoomCamera(CameraZoomMode.Out);
            }
        }

        private void ZoomCamera(CameraZoomMode mode)
        {
            int modeMultiplier = mode == CameraZoomMode.In ? -1 : 1;
            float newSize = _camera.orthographicSize + _zoomStep * modeMultiplier;
            _camera.orthographicSize = Mathf.Clamp(newSize, _minCameraSize, _maxCameraSize);
            
            _camera.transform.position = ClampCamera(_camera.transform.position);
        }

        // Because the size of camera may be changed by zoom in or zoom out, so we need to use this to control
        private Vector3 ClampCamera(Vector3 targetPosition)
        {
            var position = targetPosition.ToString();
            // Debug.Log($"Wes - [BuildingCameraController] targetPosition = {position}");

            float cameraHeight = _camera.orthographicSize * 2f;
            float cameraWidth = cameraHeight * _camera.aspect;

            float minX = _mapMinX + cameraWidth;
            float maxX = _mapMaxX - cameraWidth;

            // Debug.Log($"Wes - _mapMinX = {_mapMinX}, cameraWidth = {cameraWidth} --> minX = {minX}");
            // Debug.Log($"Wes - _mapMaxX = {_mapMaxX}, cameraWidth = {cameraWidth} --> maxX = {maxX}");

            float minY = _mapMinY + cameraHeight;
            float maxY = _mapMaxY - cameraHeight;

            float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
            float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

            return new Vector3(newX, newY, transform.position.z);
        }
    }
}
