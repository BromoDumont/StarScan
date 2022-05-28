using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerScript _PlayerScript;

    public int maxPoints;

    void Awake()
    {
        LoadData();
    }

    void Update()
    {
        //inGamePointsTxt.text = _PlayerScript.roundPoint.ToString();
    }

    public void Pause()
    {
        if (Time.timeScale > 0)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void DeathScreen()
    {
        //plObj.SetActive(false);
        //deathScreenObj.SetActive(true);
        //deathMaxPoints.text = maxPoints.ToString();
        //deathRoundPoints.text = _PlayerScript.roundPoint.ToString();
    }

    public void GameRestart()
    {
        SaveData();
        SceneManager.LoadScene(0);
        _PlayerScript.roundPoint = 0;
        //deathScreenObj.SetActive(false);
    }

    public void SaveData()
    {
        SaveSystem.SavePointsData(this);
    }
    
    public void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/StarScan.wts"))
        {
            PointsData data = SaveSystem.LoadPointsData();
            maxPoints = data._maxPoint;
        }
    }
}
