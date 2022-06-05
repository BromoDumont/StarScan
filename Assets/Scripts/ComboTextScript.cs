using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboTextScript : MonoBehaviour
{
    [Tooltip ("Conecta o script 'PlayerScript' com o script 'GameManager'.")] [SerializeField] 
    private PlayerScript _PlayerScript;

    [Tooltip ("Caixa de texto com o valor do combo.")] [SerializeField]
    private TextMeshPro comboTxt;

    void Start()
    {
        _PlayerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
        comboTxt.text = _PlayerScript.comboPoints.ToString();
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
