using UnityEngine;

namespace Project.Movement
{
    public class PanZoom : MonoBehaviour {
        Vector3 touchStart;

        public bool MouseScrollForward => Input.GetAxis("Mouse ScrollWheel") > 0f;

        public bool MouseScrollBackward => Input.GetAxis("Mouse ScrollWheel") < 0f;

        [SerializeField]
        private Camera _sceneCamera = null;

        [SerializeField]
        private float _zoomStep = 1;

        [SerializeField]
        private int _minCameraSize = 1;

        [SerializeField]
        private int _maxCameraSize = 11;

        [SerializeField]
        private SpriteRenderer _mapRenderer = null;

        private float _mapMinX, _mapMaxX, _mapMinY, _mapMaxY;
        
        private void Awake()
        {
            SetCameraBound();
        }
        
        private void SetCameraBound()
        {
            _mapMinX = _mapRenderer.transform.position.x - _mapRenderer.bounds.size.x / 2f;
            _mapMaxX = _mapRenderer.transform.position.x + _mapRenderer.bounds.size.x / 2f;

            _mapMinY = _mapRenderer.transform.position.y - _mapRenderer.bounds.size.y / 2f;
            _mapMaxY = _mapRenderer.transform.position.y + _mapRenderer.bounds.size.y / 2f;
        }

        private void Update () 
        {
            if(Input.GetMouseButtonDown(0))
            {
                touchStart = _sceneCamera.ScreenToWorldPoint(Input.mousePosition);
            }

            if(Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                ZoomCameraByTouch(difference * 0.01f);
            }
            else if(Input.GetMouseButton(0))
            {
                Vector3 direction = touchStart - _sceneCamera.ScreenToWorldPoint(Input.mousePosition);
                
                // _sceneCamera.transform.position += direction;
                _sceneCamera.transform.position = ClampCamera(_sceneCamera.transform.position + direction);
            }

            if (MouseScrollForward)
            {
                ZoomCameraByMouse(CameraZoomMode.In);
            }

            if (MouseScrollBackward)
            {
                ZoomCameraByMouse(CameraZoomMode.Out);
            }
        }

        void ZoomCameraByTouch(float increment)
        {
            _sceneCamera.orthographicSize = Mathf.Clamp(_sceneCamera.orthographicSize - increment, _minCameraSize, _maxCameraSize);
        }

        
        private void ZoomCameraByMouse(CameraZoomMode mode)
        {
            int modeMultiplier = mode == CameraZoomMode.In ? -1 : 1;
            float newSize = _sceneCamera.orthographicSize + _zoomStep * modeMultiplier;
            _sceneCamera.orthographicSize = Mathf.Clamp(newSize, _minCameraSize, _maxCameraSize);
            
            _sceneCamera.transform.position = ClampCamera(_sceneCamera.transform.position);
        }

        private Vector3 ClampCamera(Vector3 targetPosition)
        {
            float cameraHeight = _sceneCamera.orthographicSize;
            float cameraWidth = cameraHeight * _sceneCamera.aspect;
        
            float minX = _mapMinX + cameraWidth;
            float maxX = _mapMaxX - cameraWidth;
        
            float minY = _mapMinY + cameraHeight;
            float maxY = _mapMaxY - cameraHeight;
        
            float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
            float newY = Mathf.Clamp(targetPosition.y, minY, maxY);
        
            return new Vector3(newX, newY, transform.position.z);
        }

    }
}