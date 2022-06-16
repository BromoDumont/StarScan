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
    [Tooltip ("Camera object.")] [SerializeField]
    private GameObject cameraObj;
    [Tooltip ("Continue button.")]
    public Button deathContinueBtn;
    [Tooltip ("Define if this round is a continuation of the previous one")]
    public bool isContinuation;
    [Tooltip ("Combo text prefab.")] [SerializeField]
    public Transform comboTxtPrefab;

    [Header ("--------------------------------------")]
    [Space]
    [Tooltip ("Hook prefab.")] [SerializeField]
    private GameObject hookPrefab;
    [Tooltip ("Current scans quantity.")]
    public int roundScans;
    [Tooltip ("Default scans font size.")] [SerializeField]
    private float defaultScansFontSize;
    [Tooltip ("Current combo quantity.")]
    public int comboPoints;

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
    [Tooltip ("Max scans combo value in death hud")]
    public TextMeshProUGUI deathMaxComboTxtBox;
    [Tooltip ("Max scans combo in round value in death hud")]
    public TextMeshProUGUI deathMaxRoundComboTxtBox;

    [Header ("--------------------------------------")]
    [Space]
    [Tooltip ("Max scans.")]
    public int maxScans;
    [Tooltip ("Max scan combo")]
    public int maxCombo;
    [Tooltip ("Max Round Combo")]
    public int maxRoundCombo;
    [Tooltip ("Last round scans.")]
    public int lastRoundScans;
    [Tooltip ("Last max round combo")]
    public int lastMaxRoundCombo;

    void Awake()
    {
        Time.timeScale = 1;
        LoadData();

        if (isContinuation == true)
        {
            roundScans = lastRoundScans - 1;
            maxRoundCombo = lastMaxRoundCombo;
            isContinuation = false;
            deathContinueBtn.interactable = false;
        }
        else
        {
            deathContinueBtn.interactable = true;
        }

        defaultScansFontSize = defaultCurrentScansTxtBox.fontSize;
        defaultMaxScansTxtBox.text = maxScans.ToString();
        defaultMaxComboTxtBox.text = maxCombo.ToString();
    }

    void FixedUpdate()
    {
        cameraObj.transform.position = new Vector3(_PlayerScript.cameraXAxis + 4, 0, -10);

        if (defaultCurrentScansTxtBox.fontSize > defaultScansFontSize)
        {
            defaultCurrentScansTxtBox.fontSize -= 50 * Time.deltaTime;
        }
    }

    public void AddPoint()
    {
        roundScans++;
        defaultCurrentScansTxtBox.text = roundScans.ToString();
        defaultCurrentScansTxtBox.fontSize = defaultScansFontSize + (defaultScansFontSize/4);

        if (maxScans < roundScans)
        {
            maxScans = roundScans;
            defaultMaxScansTxtBox.text = maxScans.ToString();
        }
    }

    public void NewHook()
    {
        Instantiate(hookPrefab, new Vector2(_PlayerScript.hookObj.transform.position.x + 16.5f, Random.Range(-17, 15)), Quaternion.identity);
    }

    public void ComboVerify()
    {
        if (_PlayerScript.comboShields >= 0)
        {
            comboPoints++;
            if (comboPoints > 2)
            {
                Instantiate(comboTxtPrefab, _PlayerScript.plBody.position, Quaternion.identity);

                if (comboPoints > maxCombo)
                {
                    maxCombo = comboPoints;
                    defaultMaxComboTxtBox.text = maxCombo.ToString();
                }

                if (comboPoints > maxRoundCombo)
                {
                    maxRoundCombo = comboPoints;
                    deathMaxRoundComboTxtBox.text = maxRoundCombo.ToString();
                }
            }
        }
    }

    public void Pause()
    {
        if (Time.timeScale > 0)
        {
            Time.timeScale = 0;
            pauseScansTxtBox.text = roundScans.ToString();
            pauseMaxScansTxtBox.text = maxScans.ToString();
            pauseMaxComboTxtBox.text = maxCombo.ToString();

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
        deathMaxScansTxt.text = maxScans.ToString();
        deathRoundScansTxt.text = roundScans.ToString();
        deathMaxComboTxtBox.text = maxCombo.ToString();

        deathUIObj.SetActive(true);
        defaultUIObj.SetActive(false);

        _PlayerScript.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        _PlayerScript.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
    }

    public void GameRestart(bool isContinuation)
    {
        lastRoundScans = roundScans;
        lastMaxRoundCombo = maxRoundCombo;
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
        SaveSystem.SaveScansData(this);
    }
    
    public void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/StarScan.wts"))
        {
            ScansData data = SaveSystem.LoadScansData();
            maxScans = data._maxScans;
            maxCombo = data._maxCombo;
            lastRoundScans = data._lastRoundScans;
            isContinuation = data._isContinuation;
        }
    }
}
