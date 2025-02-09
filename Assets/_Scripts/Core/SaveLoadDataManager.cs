using _Scripts.Interfaces;
using _Scripts.ScriptableObject;
using UnityEngine;

namespace _Scripts.Core
{
    public class SaveLoadDataManager : MonoBehaviour, IDataSaver
    {
        [Header("Name of the save folder")]
        [SerializeField]
        private string SaveProfileName;
        [SerializeField]
        private PlayerValues playerValues;
        
        private GameData _data;
        private FileStreamHandler _fileStreamHandler;

        void Start()
        {
            _fileStreamHandler = new FileStreamHandler(Application.dataPath, SaveProfileName);
            LoadGame();    
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        public void SaveGame()
        {
            _data = new GameData
            {
                Distance = playerValues.Distance,
                PlayerScore = playerValues.Score
            };
            _fileStreamHandler.Save(_data, SaveProfileName);
        }

        public void LoadGame()
        {
            _data = _fileStreamHandler.Load(SaveProfileName);
            playerValues.SetScoreValue(_data.PlayerScore);
            playerValues.SetDistanceValue(_data.Distance);
        }
    }
}
