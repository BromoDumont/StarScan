using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour
{
    private enum State
    {
        Hook,
        HookCache
    }
    [Tooltip ("Armazena o estado do código")] [SerializeField] private State state;

    void OnTriggerEnter2D(Collider2D collider)
    {
        switch(state)
        {
            case State.HookCache:
                if (collider.CompareTag("Player"))
                {
                    //Kill Player
                }
            break;
        }
    }

    public IEnumerator AutoDestroy()
    {
        switch(state)
        {
            case State.Hook:
                SpriteRenderer hookSprite = GetComponent<SpriteRenderer>();
                hookSprite.color = new Color(1, 1, 1, 0.25f);
        
                CircleCollider2D hookColider = GetComponent<CircleCollider2D>();
                hookColider.enabled = false;
        
                yield return new WaitForSeconds(5);
                Destroy(this.gameObject);
            break;
        }
    }
}
