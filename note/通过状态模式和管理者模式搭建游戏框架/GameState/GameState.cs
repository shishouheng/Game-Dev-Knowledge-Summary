using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState : MonoBehaviour
{
    public virtual void Initialize() { }

    public virtual void OnEnter() { }

    public virtual void OnUpdate() { }

    public virtual void OnExit() { }
}
