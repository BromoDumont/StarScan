using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    #region Points System Data
        public static void SavePointsData(GameManager _GameManager)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            //Save locale in system
            string RunPatch = Application.persistentDataPath + "/StarScan.wts";
            //Create save archive
            FileStream stream = new FileStream(RunPatch, FileMode.Create);

            PointsData data = new PointsData(_GameManager);

            formatter.Serialize(stream, data);
            stream.Close();

            Debug.Log("Game Saved Successfully");
        }

        public static PointsData LoadPointsData()
        {
            string RunPatch = Application.persistentDataPath + "/StarScan.wts";
            if (File.Exists(RunPatch))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(RunPatch, FileMode.Open);

                PointsData Data = formatter.Deserialize(stream) as PointsData;
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