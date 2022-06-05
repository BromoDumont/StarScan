using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Tooltip ("Conecta o script 'PlayerScript' com o script 'GameManager'.")] [SerializeField] 
    private PlayerScript _PlayerScript;

    [Tooltip ("Objeto que armazena os elementos padrões da UI quando o jogo não está pausado")] [SerializeField]
    private GameObject defaultUIObj;

    [Tooltip ("Objeto que armazena os elementos da UI quando o jogo está pausado")] [SerializeField]
    private GameObject pauseUIObj;

    [Tooltip ("Objeto que armazena a tela de morte")] [SerializeField]
    private GameObject deathUIObj;

    [Tooltip ("Caixa de texto que mostra o recorde de pontos")] [SerializeField]
    private TextMeshProUGUI deathMaxPointsTxt;

    [Tooltip ("Caixa de texto que mostra a quantidade de pontos da ultima rodada")] [SerializeField]
    private TextMeshProUGUI roundPointsTxt;

    [Tooltip ("Monitor do timeScale")] [SerializeField]
    private float timeScaleMonitor;

    public int maxPoints;

    void Awake()
    {
        LoadData();
    }

    void Update()
    {
        timeScaleMonitor = Time.timeScale;
    }

    public void Pause()
    {
        if (Time.timeScale > 0)
        {
            pauseUIObj.SetActive(true);
            defaultUIObj.SetActive(false);
            Time.timeScale = 0;
        }
        else
        {
            pauseUIObj.SetActive(false);
            defaultUIObj.SetActive(true);
            Time.timeScale = 1;
        }
    }

    public void DeathScreen()
    {
        deathUIObj.SetActive(true);
        defaultUIObj.SetActive(false);
        _PlayerScript.gameObject.SetActive(false);

        deathMaxPointsTxt.text = maxPoints.ToString();
        roundPointsTxt.text = _PlayerScript.roundPoints.ToString();
    }

    public void GameRestart()
    {
        SaveData();
        SceneManager.LoadScene(1);
    }

    public void HomeScreen()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void InfinityMode()
    {
        SceneManager.LoadScene(1);
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
