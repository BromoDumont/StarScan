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

    [Tooltip ("Variavel que armazena a quantidade máxima de pontos já obtidos por um usuario.")]
    public int maxPoints;

    [Tooltip ("Pontos da ultima rodada.")]
    public int lastRoundPoints;

    [Tooltip ("Informa se essa é a continuação de uma rodadaa anterior ou não")]
    public bool isContinuation;

    [Tooltip ("Define se o player pode ou não prescionar o botão continue depois de morrer.")]
    public Button deathContinueBtn;

    void Awake()
    {
        Time.timeScale = 1;
        LoadData();

        if (isContinuation == true)
        {
            _PlayerScript.roundPoints = lastRoundPoints - 1;
            isContinuation = false;
            deathContinueBtn.interactable = false;
        }
        else
        {
            deathContinueBtn.interactable = true;
        }
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

        deathMaxPointsTxt.text = maxPoints.ToString();
        deathRoundPointsTxt.text = _PlayerScript.roundPoints.ToString();

        _PlayerScript.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        _PlayerScript.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
    }

    public void GameRestart(bool isContinuation)
    {
        lastRoundPoints = _PlayerScript.roundPoints;
        this.isContinuation = isContinuation;
        SaveData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CallNewScene(int sceneValue)
    {
        SceneManager.LoadScene(sceneValue);
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
            lastRoundPoints = data._lastRoundPoints;
            isContinuation = data._isContinuation;
        }
    }
}
