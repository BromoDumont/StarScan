using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    [Tooltip ("GameManager script.")] [SerializeField] 
    private GameManager _GameManager;

    [Header ("--------------------------------------")]
    [Space]
    [Tooltip ("Hook cache prefab.")]
    public GameObject hookCacheObj;
    [Tooltip ("Player angulas speed.")] [SerializeField]
    private float plAngularVel;

    [Header ("Debug Data----------------------------")]
    [Space]
    [Tooltip ("Quantity of combo shields.")]
    public int comboShields;
    [Tooltip ("Define if player is on hook.")]
    public bool plOnHook;

    #region Variaveis Privadas
        [HideInInspector] public Rigidbody2D plBody; //Component that apply physics in player object.
        private LineRenderer hookLine; //Component that makes the lines between player and hook.
        private TrailRenderer trailComponent; //Component that make player trail.
        [HideInInspector] public GameObject hookObj; //Hook object.
        [HideInInspector] public float cameraXAxis; //Camera X-Axis value.
    #endregion

    void Awake()
    {
        plBody = GetComponent<Rigidbody2D>();
        hookLine = GetComponent<LineRenderer>();
        trailComponent = GetComponent<TrailRenderer>();
    }

    void FixedUpdate()
    {
        hookLine.SetPosition(0, plBody.position);

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

    void GetHook()
    {
        _GameManager.NewHook();
        _GameManager.AddPoint();
        _GameManager.ComboVerify();

        comboShields = 1;
        hookCacheObj.transform.position = plBody.position;

        plAngularVel = Mathf.Lerp(150,500,Mathf.InverseLerp(1,100,_GameManager.roundScans));
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
                _GameManager.comboPoints = 0;
            }
        }
    }

    void OnBecameInvisible()
    {
         _GameManager.DeathScreen();
    }
}