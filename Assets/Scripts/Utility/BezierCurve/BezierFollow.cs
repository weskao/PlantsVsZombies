using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketLobby.Utility
{
    // reference: https://www.youtube.com/watch?v=11ofnLOE8pw
    public class BezierFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform[] _routes = null;
        
        [SerializeField]
        private bool _coroutineAllowed;

        private Coroutine _followRouteCoroutine;

        private List<Vector2> _nodePositionList = new List<Vector2>();

        private int _routeIndexToGo;
        private float _tParam;
        private Vector2 _targetObjectPosition;
        private float _speedModifier;
        private const int ROUTES_CHILD_COUNT = 4;

        private void Start()
        {
            _routeIndexToGo = 0;
            _tParam = 0f;
            _speedModifier = 0.5f;
            // _coroutineAllowed = true;
        }

        private void Update()
        {
            if (_coroutineAllowed && IsExistAnyRoute())
            {
                Init();
                _followRouteCoroutine = StartCoroutine(GoByTheRoute(_routeIndexToGo));
            }
        }

        private void Init()
        {
            _nodePositionList.Clear();
            StopExistedCoroutine();
        }

        private void StopExistedCoroutine()
        {
            if (_followRouteCoroutine != null)
            {
                StopCoroutine(_followRouteCoroutine);
            }
        }
        
        private IEnumerator GoByTheRoute(int routeNumber)
        {
            _coroutineAllowed = false;
            SetNodePositionList(routeNumber);

            while (_tParam < 1)
            {
                _tParam += Time.deltaTime * _speedModifier;

                _targetObjectPosition = Mathf.Pow(1 - _tParam, 3) * _nodePositionList[0] +
                                        3 * Mathf.Pow(1 - _tParam, 2) * _tParam * _nodePositionList[1] +
                                        3 * (1 - _tParam) * Mathf.Pow(_tParam, 2) * _nodePositionList[2] +
                                        Mathf.Pow(_tParam, 3) * _nodePositionList[3];

                transform.position = _targetObjectPosition;
                
                yield return new WaitForEndOfFrame();
            }

            _tParam = 0f;

            _routeIndexToGo += 1;

            if (_routeIndexToGo > _routes.Length - 1)
            {
                _routeIndexToGo = 0;
            }

            _coroutineAllowed = true;
        }

        private void SetNodePositionList(int routeNumber)
        {
            for (var i = 0; i < ROUTES_CHILD_COUNT; i++)
            {
                _nodePositionList.Add(_routes[routeNumber].GetChild(i).position);
            }
        }
        
        private bool IsExistAnyRoute()
        {
            return _routes.Length > 0;
        }
    }
}