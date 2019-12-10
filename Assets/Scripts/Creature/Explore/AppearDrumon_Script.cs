/*----------------------------------------------------------*/
//  file:      AppearDrumon_Scripts.cs		                    |
//				 											                    |
//  brief:    ドラモンを出現させるスクリプト		            | 
//															                    |
//  date:	2019.11.27									            |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using UniRx.Triggers;


// ドラモンを出現させるクラス
public class AppearDrumon_Script : MonoBehaviour
{
    [Tooltip("出現させるドラモンの最大数")]
    [SerializeField]
    private int m_maxAppear;

    [Tooltip("出現させるドラモンのPrefabを入れる")]
    [SerializeField]
    private GameObject m_drumon;

    [Tooltip("次に出現するまでの時間")]
    [SerializeField]
    private float m_appearNextTime;

    [Tooltip("出現する最大数")]
    private int m_maxDrumon;

    [Tooltip("DrumonList")]
    [SerializeField]
    private GameObject m_drumonList;

    // 現在出現している数
    private int m_currentNumberOfDrumons;
    // 計測用フィールド
    private float m_elapesdTime;

    // Start is called before the first frame update
    void Start()
    {
        // 最大数を設定する
        m_maxDrumon = m_maxAppear;
        // 計測用フィールドを初期化する
        m_elapesdTime = 0f;

        if (m_drumon.GetComponent<NavMeshAgent>() == null && m_drumon.GetComponent<NavMeshController_Script>() == null)
        {
            m_drumon.AddComponent<NavMeshAgent>();
            m_drumon.AddComponent<NavMeshController_Script>();
        }
    }

    private void Update()
    {
        // 最大出現数に達しているなら処理しない
        if (m_currentNumberOfDrumons >= m_maxDrumon)
            return;

        // 経過時間
        m_elapesdTime += Time.deltaTime;

        // 経過時間が過ぎたら
        if(m_elapesdTime > m_appearNextTime)
        {
            m_elapesdTime = 0f;

            AppearDrumon();
        }
    }


    private void AppearDrumon()
    {
        // ドラモンの向きをランダムに決定する
        var randomRotationY = Random.value * 360f;
        // 出現座標に補正をかける
        var randomPos = Random.Range(-10, 10);
        var position = new Vector3(randomPos, 0, randomPos);

        // 生成する
        var go = Instantiate(m_drumon, transform.position+position,
            Quaternion.Euler(0f, randomRotationY, 0f)
            );

        // 一つのオブジェクトにまとめる
        go.transform.parent = m_drumonList.transform;
        
        // 出現数を加算する
        m_currentNumberOfDrumons++;
    }
}
