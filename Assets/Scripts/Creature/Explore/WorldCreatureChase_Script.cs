/*----------------------------------------------------------*/
//  file:      WorldCreatureChase_Script.cs                     |
//				 											                    |
//  brief:    ワールド上のDrumonの状態遷移のスクリプト  |
//															                    |
//  date:	2019.12.24									            |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCreatureChace_Script : WorldCreatureState_Script
{
    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="controller">親オブジェクト</param>
    public override void Initialize(NavMeshController_Script controller)
    {
        m_controller = controller;

        // 到達フラグを倒す
        m_controller.IsArrived = false;
        // 経過時間を初期化する
        m_controller.ResetElapsedTime();

        // 再生するアニメーションを設定する
        m_controller.Animator.SetBool("IsWalk", true);

        // NavMeshAgentを再開する
        m_controller.Agent.isStopped = false;
    }
    
    
    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns>true=継続する false=状態変更</returns>
    public override StateID Execute()
    {
        // 対象が非アクティブならIdleにする
        if (m_controller.ChaseTarget.activeSelf == false)
            return StateID.STATE_IDLE;

        // 追いかけるターゲット座標を更新する
        m_controller.Agent.destination = m_controller.ChaseTarget.transform.position;

        // 継続する
        return StateID.CONTINUE;
    }
    
    
    /// <summary>
    /// 終了処理
    /// </summary>
    public override void Dispose()
    {
        m_controller = null;
    }
}
