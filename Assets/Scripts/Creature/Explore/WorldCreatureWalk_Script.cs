using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCreatureWalk_Script : WorldCreatureState_Script
{

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="controller"></param>
    public override void Initialize(NavMeshController_Script controller)
    {
        m_controller = controller;

        m_controller.IsArrived = false;
        m_controller.ResetElapsedTime();
    }

    
    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns></returns>
    public override bool Execute()
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// 終了処理
    /// </summary>
    public override void Dispose()
    {
        throw new NotImplementedException();
    }
}
