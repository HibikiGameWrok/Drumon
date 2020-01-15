using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ChaseCharacter_Script : MonoBehaviour
{
    // コライダーに当たったオブジェクト
    private bool m_isFind;

    NavMeshController_Script m_controller;

    /// <summary>
    /// プロパティ
    /// </summary>
    public bool IsFind => m_isFind;

    // Start is called before the first frame update
    void Start()
    {
        m_controller = GetComponentInParent<NavMeshController_Script>();
    }


    /// <summary>
    /// Search用のコライダーに衝突時の処理
    /// </summary>
    /// <param name="col">衝突した相手</param>
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            // 見つけた
            m_isFind = true;
            
            // 状態を追いかけるにする 
            m_controller.ChangeState(m_controller.Chase);
            // ターゲットの座標を取得する
            m_controller.ChaseTargetPosition = col.transform;
            // 発見時SEを鳴らす
            AudioManager_Script.Get.PlaySE(SfxType.find);
        }
    }


    /// <summary>
    /// Search用のコライダーから離れた時の処理
    /// </summary>
    /// <param name="col">衝突した相手</param>
    private void OnTriggerExit(Collider col)
    {
        if(col.tag == "Player")
        {
            // 見失う
            m_isFind = false;
            // 状態を待機にする
            m_controller.ChangeState(m_controller.Idle);
        }
    }
}
