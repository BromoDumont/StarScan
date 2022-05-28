using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Tooltip ("Conecta o script 'GameManager' com o script 'PlayerScript'")] [SerializeField] 
    private GameManager _GameManager;

    [Tooltip ("Objeto que fica no local onde o player conectou no gancho")] [SerializeField]
    private GameObject hookCacheObj;

    [Tooltip ("Prefab do gancho")] [SerializeField]
    private GameObject hookPrefab;

    [Tooltip ("Objeto da câmera")] [SerializeField]
    private GameObject cameraObj;

    [Tooltip ("Define a velocidade angular do player")] [SerializeField]
    private float plAngularVel;

    [Header ("Debug Data----------------------------")]
    [Space]

    [Tooltip ("Armazena a quantidade de pontos da rodada atual")]
    public int roundPoint;
    public bool plOnHook;
    public float hookXAxis;

    #region Variaveis Privadas
        private Rigidbody2D plBody; //Componente que aplica física ao player.
        private LineRenderer hookLine; //Componente que fara a linha entre os ganchos e o player.
        private GameObject hookObj; //Objeto do gancho.
    #endregion

    void Awake()
    {
        plBody = GetComponent<Rigidbody2D>();
        hookLine = GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        cameraObj.transform.position = new Vector3(hookXAxis + 4, 0, -10);
        hookLine.SetPosition(0, plBody.position);

        if (plOnHook == true)
        {
            hookLine.SetPosition(1, hookObj.transform.position);
            if (hookXAxis < hookObj.transform.position.x)
            {
                hookXAxis += 0.5f;
            }
        }
        else
        {
            hookLine.SetPosition(1, plBody.position);
        }
    }

    public void AddPoint()
    {
        roundPoint++;

        if (_GameManager.maxPoints < roundPoint)
        {
            _GameManager.maxPoints = roundPoint;
        }
    }

    void GetHook()
    {
        NewHook();
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
        Instantiate(hookPrefab, new Vector2(hookObj.transform.position.x + 16.5f, Random.Range(-20, 15)), Quaternion.identity);
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
