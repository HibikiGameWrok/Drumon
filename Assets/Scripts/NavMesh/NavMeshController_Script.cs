/*----------------------------------------------------------*/
//  file:      NavMeshController_Scripts.cs		                |
//				 											                    |
//  brief:    NavMeshを制御するスクリプト		                | 
//															                    |
//  date:	2019.11.27									            |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using UniRx.Triggers;


// NavMeshControllerクラスの定義
[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshController_Script : MonoBehaviour
{
    [SerializeField]
    private Transform[] m_targets = null;
    [SerializeField]
    private float m_destinationThreshold = 0.0f;

    private NavMeshAgent m_navAgent = null;

    private int m_targetIndex = 0;

    private Vector3 CurrentTargetPosition
    {
        get
        {
            if(m_targets == null || m_targets.Length <= m_targetIndex)
            {
                return Vector3.zero;
            }
            return m_targets[m_targetIndex].position;
        }
    }


    private void Awake()
    {
        // コンポーネントを取得する
        m_navAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // 目的座標を設定する
        m_navAgent.destination = CurrentTargetPosition;

        // 更新処理　Destroyされるときに通知を切る
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                NextPosition();
            }).AddTo(gameObject);
    }

    private void NextPosition()
    {
        if (m_navAgent.remainingDistance <= m_destinationThreshold)
        {
            m_targetIndex = (m_targetIndex + 1) % m_targets.Length;

            m_navAgent.destination = CurrentTargetPosition;
        }
    }
}
