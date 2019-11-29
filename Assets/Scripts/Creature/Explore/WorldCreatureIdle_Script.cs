using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCreatureIdle_Script : WorldCreatureState_Script
{
    // 待ち時間
    [SerializeField]
    private float m_waitTime = 5f;
    
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

        m_controller.Agent.isStopped = true;
    }

    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns></returns>
    public override bool Execute()
    {
        if (m_controller.ElapsedTime > m_waitTime)
        { 
            // 状態を変更する
            m_controller.ChangeState(m_controller.Walk);
            return false;
        }
        // 到着していたら一定時間待つ
        m_controller.ElapsedTime += Time.deltaTime;
        // 継続する
        return true;
    }


    /// <summary>
    /// 終了処理
    /// </summary>
    public override void Dispose()
    {
       
    }
}
