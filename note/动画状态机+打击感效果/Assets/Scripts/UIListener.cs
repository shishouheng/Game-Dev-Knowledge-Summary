using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIListener : MonoBehaviour
{
    public PlayerStateManager manager;
    public void OnClickAttack1()
    {
        manager.ChangeState<PlayerStateAttack01>();
    }
    public void OnClickAttack2()
    {
        manager.ChangeState<PlayerStateAttack02>();
    }
}
