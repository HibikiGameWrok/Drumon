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

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            m_isFind = true;

            m_controller.ChangeState(m_controller.Chase);
            m_controller.ChaseTargetPosition = col.transform;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.tag == "Player")
        {
            m_isFind = false;

            m_controller.ChangeState(m_controller.Idle);
        }
    }
}
