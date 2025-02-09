using System;
using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Core
{
    public class ConsumableSquare : MonoBehaviour, IConsumable
    {
        [SerializeField] private int PointsValue;

        public event Action<IConsumable> OnSquareConsumed; 

        public int GetScorePoints()
        {
            OnSquareConsumed?.Invoke(this);
            return PointsValue;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}
