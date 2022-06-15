using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    #region Points System Data
        public static void SaveScansData(GameManager _GameManager)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            //Save locale in system
            string RunPatch = Application.persistentDataPath + "/StarScan.wts";
            //Create save archive
            FileStream stream = new FileStream(RunPatch, FileMode.Create);

            ScansData data = new ScansData(_GameManager);

            formatter.Serialize(stream, data);
            stream.Close();

            Debug.Log("Game Saved Successfully");
        }

        public static ScansData LoadScansData()
        {
            string RunPatch = Application.persistentDataPath + "/StarScan.wts";
            if (File.Exists(RunPatch))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(RunPatch, FileMode.Open);

                ScansData Data = formatter.Deserialize(stream) as ScansData;
                stream.Close();

                Debug.Log("Save Data Has Loaded Successfully");
                return Data;
            }
            else
            {
                Debug.Log("Save File Is Not Found" + RunPatch);
                return null;
            }
        }
    #endregion
}