using UnityEngine;

namespace Project.Movement
{
    public class TouchAndGo : MonoBehaviour
    {
        [SerializeField]
        private float _moveSpeed = 5f;

        private Rigidbody2D _rb;
        private Touch _touch;
        private Vector3 _touchPosition, _whereToMove;
        private bool _isMoving = false;
        private float previousDistanceToTouchPosition, currentDistanceToTouchPosition;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (_isMoving)
            {
                currentDistanceToTouchPosition = (_touchPosition - transform.position).magnitude;
            }

#if UNITY_EDITOR

            if (Input.GetMouseButtonDown(0))
            {
#else

            if (Input.touchCount > 0)
            {
                _touch = Input.GetTouch(0);

                if (_touch.phase == TouchPhase.Began)
                {
#endif
                previousDistanceToTouchPosition = 0;
                currentDistanceToTouchPosition = 0;
                _isMoving = true;
#if UNITY_EDITOR
                _touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#else
                    _touchPosition = Camera.main.ScreenToWorldPoint(_touch.position);
#endif
                _touchPosition.z = 0;
                _whereToMove = (_touchPosition - transform.position).normalized;
                _rb.velocity = new Vector2(_whereToMove.x * _moveSpeed, _whereToMove.y * _moveSpeed);
#if !UNITY_EDITOR
                }

#endif
            }

            if (currentDistanceToTouchPosition > previousDistanceToTouchPosition)
            {
                _isMoving = false;
                _rb.velocity = Vector2.zero;
            }

            if (_isMoving)
            {
                previousDistanceToTouchPosition = (_touchPosition - transform.position).magnitude;
            }
        }
    }
}