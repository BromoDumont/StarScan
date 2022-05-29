using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    [Tooltip ("Conecta o script 'GameManager' com o script 'PlayerScript'.")] [SerializeField] 
    private GameManager _GameManager;

    [Tooltip ("Objeto que fica no local onde o player conectou no gancho.")] [SerializeField]
    private GameObject hookCacheObj;

    [Tooltip ("Prefab do gancho.")] [SerializeField]
    private GameObject hookPrefab;

    [Tooltip ("Objeto da câmera.")] [SerializeField]
    private GameObject cameraObj;

    [Tooltip ("Define a velocidade angular do player.")] [SerializeField]
    private float plAngularVel;

    [Tooltip ("Caixa de texto que mostra a quantidade de pontos do player.")] [SerializeField]
    private TextMeshProUGUI pointsTxtBox;

    [Header ("Debug Data----------------------------")]
    [Space]

    [Tooltip ("Armazena a quantidade de pontos da rodada atual.")]
    public int roundPoints;

    [Tooltip ("Armazena a informação se o player está ou não conectado em um gancho.")]
    public bool plOnHook;

    #region Variaveis Privadas
        private Rigidbody2D plBody; //Componente que aplica física ao player.
        private LineRenderer hookLine; //Componente que fara a linha entre os ganchos e o player.
        private GameObject hookObj; //Objeto do gancho.
        private float cameraXAxis; //Valor do eixo X da camêra
    #endregion

    void Awake()
    {
        plBody = GetComponent<Rigidbody2D>();
        hookLine = GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        pointsTxtBox.text = roundPoints.ToString();
        cameraObj.transform.position = new Vector3(cameraXAxis + 4, 0, -10);
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

    public void AddPoint()
    {
        roundPoints++;

        if (_GameManager.maxPoints < roundPoints)
        {
            _GameManager.maxPoints = roundPoints;
        }
    }

    void GetHook()
    {
        NewHook();

        if (plAngularVel > 0 & plAngularVel < 301)
        {
            plAngularVel += roundPoints/4;
        }

        if (plAngularVel < 0 & plAngularVel > -301)
        {
            plAngularVel -= roundPoints/4;
        }

        if (plAngularVel > 0)
        {
            if (plAngularVel < 300)
            {
                plAngularVel += roundPoints/4;
            }
            else
            {
                if (plAngularVel < 500)
                {
                    plAngularVel += roundPoints/8;
                }
                else
                {
                    plAngularVel += roundPoints/16;
                }
            }
        }
        else
        {
            if (plAngularVel > -300)
            {
                plAngularVel -= roundPoints/4;
            }
            else
            {
                if (plAngularVel > -500)
                {
                    plAngularVel -= roundPoints/8;
                }
                else
                {
                    plAngularVel -= roundPoints/16;
                }
            }
        }
        AddPoint();

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

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Hook"))
        {
            hookObj = collider.gameObject;
            GetHook();
        }
    }

    void OnBecameInvisible()
    {
        _GameManager.GameRestart();
    }
}
