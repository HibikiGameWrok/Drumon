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
    
        // 再生するアニメ―ションを設定する
        m_controller.Animator.SetBool("IsWalk", true);  
      
        // NavMeshAgentを再開する
        m_controller.Agent.isStopped = false;
    }


    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns>true=継続する false=状態変更</returns>
    public override bool Execute()
    {
        if (m_controller.Agent.remainingDistance <= 0.3f)
        {
            // 次の座標を設定する
            m_controller.NextPosition();
            // 状態を変更する
            m_controller.Animator.SetBool("IsWalk",false);
            return false;
        }

        // 継続する
        return true;
    }


    /// <summary>
    /// 終了処理
    /// </summary>
    public override void Dispose()
    {
        m_controller = null;
    }
}
