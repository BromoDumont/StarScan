using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    [Tooltip ("Conecta o script 'PlayerScript' com o script 'GameManager'.")] [SerializeField] 
    private PlayerScript _PlayerScript;

    [Tooltip ("Objeto que armazena os elementos da UI quando o jogo está pausado")] [SerializeField]
    private GameObject pauseObj;

    [Tooltip ("Componente da imagem do botão de pause.")] [SerializeField]
    private Image pauseBtnImageComp;

    [Tooltip ("Armazena os sprites do botão de pause.")] [SerializeField]
    private Sprite[] pauseBtnSprite;

    public int maxPoints;

    void Awake()
    {
        LoadData();
    }

    void Update()
    {
        //inGamePointsTxt.text = _PlayerScript.roundPoints.ToString();
    }

    public void Pause()
    {
        if (Time.timeScale > 0)
        {
            pauseBtnImageComp.sprite = pauseBtnSprite[0];
            pauseObj.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseBtnImageComp.sprite = pauseBtnSprite[1];
            pauseObj.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void DeathScreen()
    {
        //plObj.SetActive(false);
        //deathScreenObj.SetActive(true);
        //deathMaxPoints.text = maxPoints.ToString();
        //deathRoundPointss.text = _PlayerScript.roundPoints.ToString();
    }

    public void GameRestart()
    {
        SaveData();
        SceneManager.LoadScene(0);
        _PlayerScript.roundPoints = 0;
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
