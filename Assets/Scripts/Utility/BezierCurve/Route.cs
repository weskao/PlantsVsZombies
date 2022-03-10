using UnityEngine;

namespace PocketLobby.Utility
{
    public class Route : MonoBehaviour
    {
        [SerializeField]
        private Transform[] _controlPoints;

        private Vector2 _gizmosPosition;

        private void OnDrawGizmos()
        {
            for (float t = 0; t <= 1; t += 0.02f)
            {
                _gizmosPosition = Mathf.Pow(1 - t, 3) * _controlPoints[0].position +
                                 3 * Mathf.Pow(1 - t, 2) * t * _controlPoints[1].position +
                                 3 * (1 - t) * Mathf.Pow(t, 2) * _controlPoints[2].position +
                                 Mathf.Pow(t, 3) * _controlPoints[3].position;

                Gizmos.DrawSphere(_gizmosPosition, 5f);
            }
            
            Gizmos.DrawLine(_controlPoints[0].position, _controlPoints[1].position);
            Gizmos.DrawLine(_controlPoints[2].position, _controlPoints[3].position);
        }
    }
}