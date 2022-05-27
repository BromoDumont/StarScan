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

    void Update()
    {
        switch(state)
        {
            case State.Hook:

            break;

            case State.HookCache:

            break;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
    }
}
