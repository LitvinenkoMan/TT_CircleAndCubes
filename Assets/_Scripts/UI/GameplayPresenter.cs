using System;
using System.Globalization;
using _Scripts.Interfaces;
using _Scripts.ScriptableObject;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class GameplayPresenter : MonoBehaviour, ICircleGameplayPresenter
    {
        [SerializeField] private PlayerValues playerValues;

        [Space(20), Header("UI Elements")]
        [SerializeField]
        private TMP_Text DistanceText;
        [SerializeField]
        private TMP_Text ScoreText;

        private void OnEnable()
        {
            playerValues.OnDistanceChanged += VisualizePlayerDistance;
            playerValues.OnScoreChanged += VisualizePlayerScore;
        }

        private void OnDisable()
        {
            playerValues.OnDistanceChanged -= VisualizePlayerDistance;
            playerValues.OnScoreChanged -= VisualizePlayerScore;
        }


        void Start()
        {
            VisualizePlayerScore(playerValues.Score);
            VisualizePlayerDistance(playerValues.Distance);
        }

        public void VisualizePlayerScore(int score)
        {
            ScoreText.text = score.ToString();
        }

        public void VisualizePlayerDistance(float distance)
        {
            DistanceText.text = distance.ToString();
        }
    }
}
