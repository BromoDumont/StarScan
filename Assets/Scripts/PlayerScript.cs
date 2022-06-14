using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    [Tooltip ("GameManager script.")] [SerializeField] 
    private GameManager _GameManager;
    [Tooltip ("Camera object.")] [SerializeField]
    private GameObject cameraObj;

    [Header ("--------------------------------------")]
    [Space]
    [Tooltip ("Hook cache prefab.")]
    public GameObject hookCacheObj;
    [Tooltip ("Hook prefab.")] [SerializeField]
    private GameObject hookPrefab;
    [Tooltip ("Player angulas speed.")] [SerializeField]
    private float plAngularVel;

    [Header ("Debug Data----------------------------")]
    [Space]
    [Tooltip ("Current scans quantity.")]
    public int roundPoints;
    [Tooltip ("Current combo quantity.")]
    public int comboPoints;
    [Tooltip ("Quantity of combo shields.")]
    public int comboShields;
    [Tooltip ("Default scans font size.")] [SerializeField]
    private float defaultScansFontSize;
    [Tooltip ("Define if player is on hook.")]
    public bool plOnHook;

    #region Variaveis Privadas
        private Rigidbody2D plBody; //Component that apply physics in player object.
        private LineRenderer hookLine; //Component that makes the lines between player and hook.
        private TrailRenderer trailComponent; //Component that make player trail.
        private GameObject hookObj; //Hook object.
        private float cameraXAxis; //Camera X-Axis value.
    #endregion

    void Awake()
    {
        plBody = GetComponent<Rigidbody2D>();
        hookLine = GetComponent<LineRenderer>();
        trailComponent = GetComponent<TrailRenderer>();

        defaultScansFontSize = _GameManager.defaultCurrentScansTxtBox.fontSize;
    }

    void FixedUpdate()
    {
        cameraObj.transform.position = new Vector3(cameraXAxis + 4, 0, -10);
        hookLine.SetPosition(0, plBody.position);

        if (_GameManager.defaultCurrentScansTxtBox.fontSize > defaultScansFontSize)
        {
            _GameManager.defaultCurrentScansTxtBox.fontSize -= 50 * Time.deltaTime;
        }

        if (plOnHook == true)
        {
            hookLine.SetPosition(1, hookObj.transform.position);
            if (cameraXAxis < hookObj.transform.position.x)
            {
                cameraXAxis += 0.5f;
            }
        }
        else
        {
            hookLine.SetPosition(1, plBody.position);
        }
    }

    public void AddPoint()
    {
        roundPoints++;
        _GameManager.defaultCurrentScansTxtBox.text = roundPoints.ToString();
        _GameManager.defaultCurrentScansTxtBox.fontSize = defaultScansFontSize + (defaultScansFontSize/4);

        if (_GameManager.maxScans < roundPoints)
        {
            _GameManager.maxScans = roundPoints;
        }
    }

    void GetHook()
    {
        NewHook();
        AddPoint();

        if (comboShields >= 0)
        {
            comboPoints++;
            if (comboPoints > 2)
            {
                NewComboText();
            }
        }

        comboShields = 1;
        hookCacheObj.transform.position = plBody.position;

        plAngularVel = Mathf.Lerp(150,500,Mathf.InverseLerp(0,100,roundPoints));
        trailComponent.time = Mathf.Lerp(0.11f, 2.35f,Mathf.InverseLerp(500,150,plAngularVel));

        if (plBody.velocity.y > 0 & plAngularVel > 0 || plBody.velocity.y < 0 & plAngularVel < 0)
        {
            plAngularVel = plAngularVel * -1f;
        }
        
        plBody.velocity = new Vector2(0, 0);
        plBody.centerOfMass = (new Vector2(hookObj.transform.position.x, hookObj.transform.position.y) - plBody.position);
        plBody.angularVelocity = plAngularVel;

        plOnHook = true;
    }

    public void ReleaseHook()
    {
        plBody.centerOfMass = new Vector2(0,0);
        plBody.angularVelocity = 0;
        plBody.rotation = 0;

        HookScript _HookScript = hookObj.GetComponent<HookScript>();
        StartCoroutine(_HookScript.AutoDestroy());

        plOnHook = false;
    }

    void NewHook()
    {
        Instantiate(hookPrefab, new Vector2(hookObj.transform.position.x + 16.5f, Random.Range(-17, 15)), Quaternion.identity);
    }

    void NewComboText()
    {
        Instantiate(_GameManager.comboTxtPrefab, plBody.position, Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Hook"))
        {
            hookObj = collider.gameObject;
            GetHook();
        }

        if (collider.gameObject.CompareTag("HookCache"))
        {
            comboShields--;
            if (comboShields < 0)
            {
                comboPoints = 0;
            }
        }
    }

    void OnBecameInvisible()
    {
         _GameManager.DeathScreen();
    }
}