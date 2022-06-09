using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboTextScript : MonoBehaviour
{
    [Tooltip ("Objeto do player")] [SerializeField]
    private GameObject playerObj;

    [Tooltip ("Conecta o script 'PlayerScript' com o script 'GameManager'.")] [SerializeField] 
    private PlayerScript _PlayerScript;

    [Tooltip ("Caixa de texto com o valor do combo.")] [SerializeField]
    private TextMeshPro comboTxt;

    [Tooltip ("Variavel que modificara a posição do texto do combo no eixo Y")] [SerializeField]
    private float verticalVariable;

    void Start()
    {
        playerObj = GameObject.Find("Player");
        _PlayerScript = playerObj.GetComponent<PlayerScript>();
        comboTxt.text = _PlayerScript.comboPoints.ToString();
    }

    void Update()
    {   
        if (playerObj.GetComponent<Rigidbody2D>().angularVelocity > 0)
        {
            verticalVariable += Time.deltaTime / 8;
        }
        else
        {
            verticalVariable -= Time.deltaTime / 8;
        }

        if (comboTxt.fontSize < 1)
        {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y + verticalVariable);
    }
}
