/*----------------------------------------------------------*/
//  file:      WorldCreatureState_Script.cs                     |
//				 											                    |
//  brief:    ワールド上のDrumonの状態遷移のスクリプト  |
//															                    |
//  date:	2019.11.29									            |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCreatureIdle_Script : WorldCreatureState_Script
{
    // 待ち時間
    private float m_waitTime = 3f;
    
    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initialize(NavMeshController_Script contoller)
    {
        m_controller = contoller;

        // 経過時間を初期化する
        m_controller.ResetElapsedTime();
        // 到着した
        m_controller.IsArrived = true;
    
        // アニメーターを設定する
        m_controller.Animator.SetBool("IsWalk", false);    
        
        // NavMeshAgentを停止させる
        m_controller.Agent.isStopped = true;
    }

    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns>true=継続する false=状態変更</returns>
    public override StateID Execute()
    {
        if (m_controller.ElapsedTime > m_waitTime)
        { 
            // 状態を変更する
            return StateID.STATE_WALK;
        }

        // 到着していたら一定時間待つ
        m_controller.ElapsedTime += Time.deltaTime;
       
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
