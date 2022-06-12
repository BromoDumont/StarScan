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

    [Tooltip ("velocidade angular do player.")] [SerializeField]
    private float plAngularVel;

    [Tooltip ("Caixa de texto com a quantidade de pontos do player.")] [SerializeField]
    private TextMeshProUGUI pointsTxtBox;

    [Tooltip ("Caixa de texto com a quantidade maxima de pontos já obtidas em uma rodada.")] [SerializeField]
    private TextMeshProUGUI maxPointsTxtBox;

    [Tooltip ("Objeto da caixa de texto dos combos.")] [SerializeField]
    private Transform comboTxtObj;

    [Header ("Debug Data----------------------------")]
    [Space]

    [Tooltip ("Quantidade de pontos da rodada atual.")]
    public int roundPoints;

    [Tooltip ("Quantidade de pontos do combo.")]
    public int comboPoints;

    [Tooltip ("Quantidade de protetores de combo.")]
    public int comboProtector;

    [Tooltip ("Tamanho padrão da fonte do texto com a pontuação,")] [SerializeField]
    private float defaultFontSize;

    [Tooltip ("Informa se o player está ou não conectado em um gancho.")]
    public bool plOnHook;

    #region Variaveis Privadas
        private Rigidbody2D plBody; //Componente que aplica física ao player.
        private LineRenderer hookLine; //Componente que fara a linha entre os ganchos e o player.
        private TrailRenderer trailComponent; //Cmponente que faz a trilha do player.
        private GameObject hookObj; //Objeto do gancho.
        private float cameraXAxis; //Valor do eixo X da camêra.
    #endregion

    void Awake()
    {
        plBody = GetComponent<Rigidbody2D>();
        hookLine = GetComponent<LineRenderer>();
        trailComponent = GetComponent<TrailRenderer>();
        defaultFontSize = pointsTxtBox.fontSize;
        maxPointsTxtBox.text = _GameManager.maxPoints.ToString();
    }

    void FixedUpdate()
    {
        cameraObj.transform.position = new Vector3(cameraXAxis + 4, 0, -10);
        hookLine.SetPosition(0, plBody.position);

        if (pointsTxtBox.fontSize > defaultFontSize)
        {
            pointsTxtBox.fontSize -= 50 * Time.deltaTime;
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
        pointsTxtBox.text = roundPoints.ToString();
        pointsTxtBox.fontSize = defaultFontSize + (defaultFontSize/4);

        if (_GameManager.maxPoints < roundPoints)
        {
            _GameManager.maxPoints = roundPoints;
        }
    }

    void GetHook()
    {
        NewHook();
        AddPoint();

        if (comboProtector >= 0)
        {
            comboPoints++;
            if (comboPoints > 2)
            {
                NewComboText();
            }
        }

        comboProtector = 1;
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
        Instantiate(comboTxtObj, plBody.position, Quaternion.identity);
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
            comboProtector--;
            if (comboProtector < 0)
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