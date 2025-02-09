using System;
using _Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

namespace _Scripts.Core
{
    public class SquareSpawner : MonoBehaviour
    {
        [SerializeField] private float TimeToSpawn;
        [SerializeField, Min(1)] private int MaxAmount;

        private GameObjectPoolQueue _pool;

        private int _activeAmount;
        private Camera _camera;

        private async void Start()
        {
            _pool = GetComponent<GameObjectPoolQueue>();
            _camera = Camera.main;
            await SpawnCube();
        }

        public async UniTask SpawnCube()
        {
            await UniTask.WaitForSeconds(3);
            if (_activeAmount < MaxAmount)
            {
                _activeAmount++;
                Random rnd = new Random();
                var placementPosition = new Vector3(
                    rnd.Next((int)(Screen.width * 0.05f), (int)(Screen.width * 0.95f)),
                    rnd.Next((int)(Screen.height * 0.05f), (int)(Screen.height * 0.95f)),
                    0);
                
                var square = _pool.GetFromPool(true);
                if (square.TryGetComponent(out ConsumableSquare squaref))
                {
                    squaref.OnSquareConsumed += ReturnSquareToPool;
                }
                square.transform.position = _camera.ScreenToWorldPoint(new Vector3(placementPosition.x, placementPosition.y, 10));
            }

            await SpawnCube();
        }

        private void ReturnSquareToPool(IConsumable obj)
        {
            _pool.AddToPool(obj.GetGameObject());
            _activeAmount--;
        }
    }
}