using System;
using UnityEngine;

namespace _Scripts.ScriptableObject
{
    [CreateAssetMenu(fileName = "PlayerValues", menuName = "Scriptable Objects/PlayerValues")]
    public class PlayerValues : UnityEngine.ScriptableObject
    {
        [SerializeField]
        private float distance;
        [SerializeField]
        private int score;

        public event Action<int> OnScoreChanged;
        public event Action<float> OnDistanceChanged;

        public float Distance => distance;
        public int Score => score;

        public void SetDistanceValue(float newValue)
        {
            distance = newValue;
            OnDistanceChanged?.Invoke(distance);
        }

        public void SetScoreValue(int newValue)
        {
            score = newValue;
            OnScoreChanged?.Invoke(score);
        }
    }

}
