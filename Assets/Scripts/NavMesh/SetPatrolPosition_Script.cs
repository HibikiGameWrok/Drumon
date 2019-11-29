using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 巡回する座標を設定するクラスの定義
public class SetPatrolPosition_Script : MonoBehaviour
{
    [Tooltip("巡回する位置を決めるオブジェクトの名前")]
    [SerializeField]
    private string m_findObjectName;
    // 初期位置
    private Vector3 m_startPosition;
    // 目的地
    private Vector3 m_destination;
    // 巡回する位置
    [SerializeField]
    private Transform[] m_patrolPositions;
    // 次に巡回する位置
    private int m_nextPatrolPosition;

    public  Transform GetPatrolPosition(int index)
    {
        return m_patrolPositions[index];
    }

    // Start is called before the first frame update
    void Awake()
    {
        // 初期位置を設定する
        m_startPosition = transform.position;
        // 巡回地点を設定する
        var patrolParent = GameObject.Find(m_findObjectName);
        m_patrolPositions = new Transform[patrolParent.transform.childCount];

        for (int i = 0; i < patrolParent.transform.childCount; i++)
        {
            m_patrolPositions[i] = patrolParent.transform.GetChild(i);
        }
    }
}
