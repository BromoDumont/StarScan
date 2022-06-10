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

    [Tooltip ("Caixa de texto que mostra a quantidade de pontos da rodada atual no menu de pausa")] [SerializeField]
    private TextMeshProUGUI pausePointsTxtBox;

    [Tooltip ("Caixa de texto que mostra o rescorde de pontos no menu de pausa")] [SerializeField]
    private TextMeshProUGUI pauseMaxPointsTxtBox;

    [Tooltip ("Caixa de texto que mostra a quantidade de pontos da ultima rodada")] [SerializeField]
    private TextMeshProUGUI deathRoundPointsTxt;
    
    [Tooltip ("Caixa de texto que mostra o recorde de pontos")] [SerializeField]
    private TextMeshProUGUI deathMaxPointsTxt;


    [Tooltip ("Monitor do timeScale")] [SerializeField]
    private float timeScaleMonitor;

    public int maxPoints;

    void Awake()
    {
        Time.timeScale = 1;
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
            Time.timeScale = 0;
            pausePointsTxtBox.text = _PlayerScript.roundPoints.ToString();
            pauseMaxPointsTxtBox.text = maxPoints.ToString();
            pauseUIObj.SetActive(true);
            defaultUIObj.SetActive(false);
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
        //_PlayerScript.gameObject.SetActive(false);
        _PlayerScript.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        _PlayerScript.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;

        deathMaxPointsTxt.text = maxPoints.ToString();
        deathRoundPointsTxt.text = _PlayerScript.roundPoints.ToString();
    }

    public void GameRestart()
    {
        SaveData();
        SceneManager.LoadScene(1);
    }

    public void HomeScreen()
    {
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
