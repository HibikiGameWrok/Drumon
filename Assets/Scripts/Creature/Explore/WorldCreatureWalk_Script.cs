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
        // 到達フラグを倒す
        m_controller.IsArrived = false;
        // 経過時間を初期化する
        m_controller.ResetElapsedTime();

        m_controller.Agent.isStopped = false;
    }

    
    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns></returns>
    public override bool Execute()
    {
        if (m_controller.Agent.remainingDistance <= 0.1f)
        {
            m_controller.NextPosition();
            // 状態を変更する
            m_controller.Animator.SetBool("IsWalk",false);
            m_controller.ChangeState(m_controller.Idle);
            return false;
        }

        m_controller.Animator.SetBool("IsWalk", true);
        return true;
    }


    /// <summary>
    /// 終了処理
    /// </summary>
    public override void Dispose()
    {
        
    }
}
