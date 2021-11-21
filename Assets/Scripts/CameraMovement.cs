using System;
using DataStruct.Enum;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Movement
{
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

        private float _mapMinX, _mapMaxX, _mapMinY, _maxMaxY;

        public bool MouseScrollForward => Input.GetAxis("Mouse ScrollWheel") > 0f;

        public bool MouseScrollBackward => Input.GetAxis("Mouse ScrollWheel") < 0f;

        private Vector3 _dragOrigin;

        private void Awake()
        {
            SetCameraBound();
            RegisterClickEvent();
        }

        private void Update()
        {
            PanCamera();
        }

        private void OnDestroy()
        {
            DeregisterClickEvent();
        }

        private void SetCameraBound()
        {
            _mapMinX = _mapRenderer.transform.position.x - _mapRenderer.bounds.size.x / 2f;
            _mapMaxX = _mapRenderer.transform.position.x + _mapRenderer.bounds.size.x / 2f;

            _mapMinY = _mapRenderer.transform.position.y - _mapRenderer.bounds.size.y / 2f;
            _maxMaxY = _mapRenderer.transform.position.y + _mapRenderer.bounds.size.y / 2f;
        }

        private void RegisterClickEvent()
        {
            _zoomInButton.onClick.AddListener(() => { Zoom(ZoomMode.In); });
            _zoomOutButton.onClick.AddListener(() => { Zoom(ZoomMode.Out); });
        }

        private void DeregisterClickEvent()
        {
            _zoomInButton.onClick.RemoveListener(() => { Zoom(ZoomMode.In); });
            _zoomOutButton.onClick.RemoveListener(() => { Zoom(ZoomMode.Out); });
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
                Zoom(ZoomMode.In);
            }

            if (MouseScrollBackward)
            {
                Zoom(ZoomMode.Out);
            }
        }

        private void Zoom(ZoomMode mode)
        {
            Debug.Log($"Wes - Zoom, mode = {mode}");
            int modeMultiplier = mode == ZoomMode.In ? -1 : 1;
            float newSize = _camera.orthographicSize + _zoomStep * modeMultiplier;
            _camera.orthographicSize = Mathf.Clamp(newSize, _minCameraSize, _maxCameraSize);
            _camera.transform.position = ClampCamera(_camera.transform.position);
        }

        // Because the size of camera may be changed by zoom in or zoom out, so we need to use this to control
        private Vector3 ClampCamera(Vector3 targetPosition)
        {
            float cameraHeight = _camera.orthographicSize;
            float cameraWidth = _camera.orthographicSize * _camera.aspect;

            float minX = _mapMinX + cameraWidth;
            float maxX = _mapMaxX - cameraWidth;

            Debug.Log($"Wes - _mapMinX = {_mapMinX}, cameraWidth = {cameraWidth} --> minX = {minX}");
            Debug.Log($"Wes - _mapMaxX = {_mapMaxX}, cameraWidth = {cameraWidth} --> maxX = {maxX}");

            float minY = _mapMinY + cameraHeight;
            float maxY = _mapMinY - cameraHeight;

            float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
            float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

            return new Vector3(newX, newY, transform.position.z);
        }
    }
}