using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace _Scripts.Core
{
    public class FileStreamHandler
    {
        private string dataDirPath = "";
        private string dataFileName = "";

        public FileStreamHandler(string dataDirPath, string dataFileName) 
        {
            this.dataDirPath = dataDirPath;
            this.dataFileName = dataFileName;
        }

        public GameData Load(string profileId) 
        {
            // base case - if the profileId is null, return right away
            if (profileId == null) 
            {
                return null;
            }

            // use Path.Combine to account for different OS's having different path separators
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            GameData loadedData = null;
            if (File.Exists(fullPath)) 
            {
                try 
                {
                    // load the serialized data from the file
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    // deserialize the data from Json back into the C# object
                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e) 
                {
                    // since we're calling Load(..) recursively, we need to account for the case where
                    // the rollback succeeds, but data is still failing to load for some other reason,
                    // which without this check may cause an infinite recursion loop.
                    // if (allowRestoreFromBackup) 
                    // {
                    //     Debug.LogWarning("Failed to load data file. Attempting to roll back.\n" + e);
                    //     bool rollbackSuccess = AttemptRollback(fullPath);
                    //     if (rollbackSuccess)
                    //     {
                    //         // try to load again recursively
                    //         loadedData = Load(profileId, false);
                    //     }
                    // }
                    // // if we hit this else block, one possibility is that the backup file is also corrupt
                    // else 
                    // {
                    //     Debug.LogError("Error occured when trying to load file at path: " 
                    //         + fullPath  + " and backup did not work.\n" + e);
                    // }
                }
            }
            return loadedData;
        }

        public void Save(GameData data, string profileId) 
        {
            // base case - if the profileId is null, return right away
            if (profileId == null) 
            {
                return;
            }

            // use Path.Combine to account for different OS's having different path separators
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            try 
            {
                // create the directory the file will be written to if it doesn't already exist
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                // serialize the C# game data object into Json
                string dataToStore = JsonUtility.ToJson(data, true);

                // write the serialized data to the file
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream)) 
                    {
                        writer.Write(dataToStore);
                    }
                }

            }
            catch (Exception e) 
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
        }
    }
}
