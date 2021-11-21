using System;
using DataStruct.Enum;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Movement
{
    public partial class CameraMovement : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera = null;

        [SerializeField]
        private float _zoomStep;

        [SerializeField]
        private int _minCameraSize;

        [SerializeField]
        private int _maxCameraSize;

        [SerializeField]
        private Button _zoomInButton = null;

        [SerializeField]
        private Button _zoomOutButton = null;

        public bool MouseScrollForward => Input.GetAxis("Mouse ScrollWheel") > 0f;
        public bool MouseScrollBackward => Input.GetAxis("Mouse ScrollWheel") < 0f;

        private Vector3 _dragOrigin;

        private void Awake()
        {
            _zoomInButton.onClick.AddListener(() => { Zoom(ZoomMode.In); });
            _zoomOutButton.onClick.AddListener(() => { Zoom(ZoomMode.Out); });
        }

        private void OnDestroy()
        {
            _zoomInButton.onClick.RemoveListener(() => { Zoom(ZoomMode.In); });
            _zoomOutButton.onClick.RemoveListener(() => { Zoom(ZoomMode.Out); });
        }

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
        }
    }
}