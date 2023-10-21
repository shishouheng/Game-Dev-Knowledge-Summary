using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    /// <summary>
    /// 项目的入口脚本，负责加载游戏，负责游戏的主逻辑更新
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //初始化系统模块管理者
        SystemModuleManager.Instance.Initialize(gameObject);

        //初始化游戏状态管理
        SystemModuleManager.Instance.AddSystemModule<GameStateManager>();

        //进入游戏初始化状态
        GameStateManager.Instance.EnterState<GameStateInitialize>();
    }
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //调用所有模块的更新方法
        SystemModuleManager.Instance.OnUpdate();
	}
}
