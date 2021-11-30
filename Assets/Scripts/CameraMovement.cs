using System;
using DataStruct.Enum;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Movement
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField]
        private Camera _sceneCamera = null;

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
            DetectTouch();
        }
        
        private void SetCameraBound()
        {
            _mapMinX = _mapRenderer.transform.position.x - _mapRenderer.bounds.size.x / 2f;
            _mapMaxX = _mapRenderer.transform.position.x + _mapRenderer.bounds.size.x / 2f;

            _mapMinY = _mapRenderer.transform.position.y - _mapRenderer.bounds.size.y / 2f;
            _mapMaxY = _mapRenderer.transform.position.y + _mapRenderer.bounds.size.y / 2f;
        }

        private void DetectTouch()
        {
            // Save position of mouse in world space when drag starts (first time clicked)
            if (Input.GetMouseButtonDown(0))
            {
                _dragOrigin = _sceneCamera.ScreenToWorldPoint(Input.mousePosition);
            }

             if (Input.GetMouseButton(0))
            {
                var newPosition = _sceneCamera.ScreenToWorldPoint(Input.mousePosition);
                var difference = _dragOrigin - newPosition;

                // Debug.Log($"Wes - _dragOrigin = {_dragOrigin}, newPosition = {newPosition} --> difference = {difference}");

                // Move the camera by that distance
                // _camera.transform.position += difference;

                _sceneCamera.transform.position = ClampCamera(_sceneCamera.transform.position + difference);

                
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
        
        void ZoomCameraByTouch(float increment)
        {
            _sceneCamera.orthographicSize = Mathf.Clamp(_sceneCamera.orthographicSize - increment, _minCameraSize, _maxCameraSize);
        }


        private void ZoomCamera(CameraZoomMode mode)
        {
            int modeMultiplier = mode == CameraZoomMode.In ? -1 : 1;
            float newSize = _sceneCamera.orthographicSize + _zoomStep * modeMultiplier;
            _sceneCamera.orthographicSize = Mathf.Clamp(newSize, _minCameraSize, _maxCameraSize);
            
            _sceneCamera.transform.position = ClampCamera(_sceneCamera.transform.position);
        }

        // Because the size of camera may be changed by zoom in or zoom out, so we need to use this to control
        private Vector3 ClampCamera(Vector3 targetPosition)
        {
            var position = targetPosition.ToString();
            // Debug.Log($"Wes - [BuildingCameraController] targetPosition = {position}");

            float cameraHeight = _sceneCamera.orthographicSize * 2f;
            float cameraWidth = cameraHeight * _sceneCamera.aspect;

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
