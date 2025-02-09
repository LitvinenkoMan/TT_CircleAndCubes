using _Scripts.Interfaces;
using _Scripts.ScriptableObject;
using UnityEngine;

namespace _Scripts.Core
{
    public class PlayerScoreCounter : MonoBehaviour
    {
        [SerializeField] private PlayerValues playerValues;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("on trigger enter from player");
            if (other.gameObject.TryGetComponent(out IConsumable consumable))
            {
                playerValues.SetScoreValue(playerValues.Score + consumable.GetScorePoints());
            }
        }
    }
}
