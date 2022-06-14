using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Tooltip ("Player Script.")] [SerializeField] 
    private PlayerScript _PlayerScript;
    [Tooltip ("Continue button.")]
    public Button deathContinueBtn;
    [Tooltip ("Define if this round is a continuation of the previous one")]
    public bool isContinuation;
    [Tooltip ("Combo text prefab.")] [SerializeField]
    public Transform comboTxtPrefab;

    [Header ("--------------------------------------")]
    [Space]
    [Tooltip ("Default hud object.")] [SerializeField]
    private GameObject defaultUIObj;
    [Tooltip ("Pause hud object.")] [SerializeField]
    private GameObject pauseUIObj;
    [Tooltip ("Death hud object.")] [SerializeField]
    private GameObject deathUIObj;

    [Header ("--------------------------------------")]
    [Space]
    [Tooltip ("Max scans text value in default hud.")] [SerializeField]
    public TextMeshProUGUI defaultMaxScansTxtBox;
    [Tooltip ("Scans text value in default hud.")] [SerializeField]
    public TextMeshProUGUI defaultCurrentScansTxtBox;
    [Tooltip ("Max scans combo value in default hud")]
    public TextMeshProUGUI defaultMaxComboTxtBox;

    [Header ("--------------------------------------")]
    [Space]
    [Tooltip ("Scans value in pause hud")] [SerializeField]
    private TextMeshProUGUI pauseScansTxtBox;
    [Tooltip ("Max scan value in pause hud")] [SerializeField]
    private TextMeshProUGUI pauseMaxScansTxtBox;
    [Tooltip ("Max scans combo value in pause hud")]
    public TextMeshProUGUI pauseMaxComboTxtBox;

    [Header ("--------------------------------------")]
    [Space]
    [Tooltip ("Scans value in death hud")] [SerializeField]
    private TextMeshProUGUI deathRoundScansTxt;
    [Tooltip ("Max scan value in death hud")] [SerializeField]
    private TextMeshProUGUI deathMaxScansTxt;
    [Tooltip ("Max runs scans combo value in death hud")]
    public TextMeshProUGUI deathRunMaxComboTxtBox;

    [Header ("--------------------------------------")]
    [Space]
    [Tooltip ("Max scans.")]
    public int maxScans;
    [Tooltip ("Max scan combo")]
    public int maxCombo;
    [Tooltip ("Last round scans.")]
    public int lastRoundScans;

    void Awake()
    {
        Time.timeScale = 1;
        LoadData();

        if (isContinuation == true)
        {
            _PlayerScript.roundPoints = lastRoundScans - 1;
            isContinuation = false;
            deathContinueBtn.interactable = false;
        }
        else
        {
            deathContinueBtn.interactable = true;
        }

        defaultMaxScansTxtBox.text = maxScans.ToString();
    }   

    public void Pause()
    {
        if (Time.timeScale > 0)
        {
            Time.timeScale = 0;
            pauseScansTxtBox.text = _PlayerScript.roundPoints.ToString();
            pauseMaxScansTxtBox.text = maxScans.ToString();
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

        deathMaxScansTxt.text = maxScans.ToString();
        deathRoundScansTxt.text = _PlayerScript.roundPoints.ToString();

        _PlayerScript.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        _PlayerScript.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
    }

    public void GameRestart(bool isContinuation)
    {
        lastRoundScans = _PlayerScript.roundPoints;
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
            maxScans = data._maxScans;
            lastRoundScans = data._lastRoundScans;
            isContinuation = data._isContinuation;
        }
    }
}
