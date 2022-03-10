using System.Collections;
using UnityEngine;

namespace PocketLobby.Utility
{
    // reference: https://www.youtube.com/watch?v=11ofnLOE8pw
    public class BezierFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform[] routes = null;

        private int _routeToGo;
        private float _tParam;
        private Vector2 _targetObjectPosition;
        private float _speedModifier;
        private bool _coroutineAllowed;

        private void Start()
        {
            _routeToGo = 0;
            _tParam = 0f;
            _speedModifier = 0.5f;
            _coroutineAllowed = true;
        }

        private void Update()
        {
            if (_coroutineAllowed)
            {
                StartCoroutine(GoByTheRoute(_routeToGo));
            }
        }

        private IEnumerator GoByTheRoute(int routeNumber)
        {
            _coroutineAllowed = false;

            // use List to replace this:
            Vector2 p0 = routes[routeNumber].GetChild(0).position;
            Vector2 p1 = routes[routeNumber].GetChild(1).position;
            Vector2 p2 = routes[routeNumber].GetChild(2).position;
            Vector2 p3 = routes[routeNumber].GetChild(3).position;

            while (_tParam < 1)
            {
                _tParam += Time.deltaTime * _speedModifier;

                _targetObjectPosition = Mathf.Pow(1 - _tParam, 3) * p0 +
                                        3 * Mathf.Pow(1 - _tParam, 2) * _tParam * p1 +
                                        3 * (1 - _tParam) * Mathf.Pow(_tParam, 2) * p2 +
                                        Mathf.Pow(_tParam, 3) * p3;

                transform.position = _targetObjectPosition;
                
                yield return new WaitForEndOfFrame();
            }

            _tParam = 0f;

            _routeToGo += 1;

            if (_routeToGo > routes.Length - 1)
            {
                _routeToGo = 0;
            }

            _coroutineAllowed = true;
        }
    }
}