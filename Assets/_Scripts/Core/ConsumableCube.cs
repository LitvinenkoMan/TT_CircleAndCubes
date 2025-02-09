using System;
using _Scripts.Interfaces;
using _Scripts.ScriptableObject;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Core
{
    public class ConsumableCube : MonoBehaviour, IConsumable
    {
        [SerializeField] private PlayerValues playerValues;
        [SerializeField] private int PointsValue;

        public int GetScorePoints()
        {
            return PointsValue;
        }
    }
}
