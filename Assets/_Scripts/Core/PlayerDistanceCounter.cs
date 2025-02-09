using _Scripts.ScriptableObject;
using UnityEngine;

namespace _Scripts.Core
{
    public class PlayerDistanceCounter : MonoBehaviour
    {
        [SerializeField] private PlayerValues playerValues;
        
        private Vector3 _previousPosition;

        private void Update()
        {
            var position = transform.position;
            if (_previousPosition != position)
            {
                playerValues.SetDistanceValue(playerValues.Distance + Vector3.Distance(position, _previousPosition));
                _previousPosition = position;
            }
        }
    }
}
