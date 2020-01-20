/*----------------------------------------------------------*/
//  file:      AppearOtso_Scripts.cs		                        |
//				 											                    |
//  brief:    Otsoを出現させるスクリプト		                    | 
//															                    |
//  date:	2019.1.9									                |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearOtso_Script : MonoBehaviour
{
    [Tooltip("出現させるOtsoの最大数")]
    [SerializeField]
    private int m_max = 1;

    [Tooltip("出現させるPrefabを入れる")]
    [SerializeField]
    private GameObject m_drumon = null;

    [Tooltip("次に出現するまでの時間")]
    [SerializeField]
    private float m_appearNextTime = 0.0f;

    [Tooltip("DrumonList")]
    [SerializeField]
    private GameObject m_drumonList = null;

    // 現在出現している数
    private int m_currentNumberOfDrumons;
    // 計測用フィールド
    private float m_elapesdTime;

    [SerializeField]
    private AddDrumonList_Script m_list = null;

    // Start is called before the first frame update
    void Start()
    {
        // 計測用フィールドを初期化する
        m_elapesdTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // 最大出現数に達しているなら処理しない
        if (m_currentNumberOfDrumons >= m_max)
            return;

        // 経過時間
        m_elapesdTime += Time.deltaTime;

        // 経過時間が過ぎたら
        if(m_elapesdTime > m_appearNextTime)
        {
            m_elapesdTime = 0f;

            AppearOtso();
        }
    }

    private void AppearOtso()
    {
        // 生成する
        var go = Instantiate(m_drumon, transform.position, 
            Quaternion.Euler(0f, 90f, 0f)
            );

        // 一つのオブジェクトにまとめる
        go.transform.parent = m_drumonList.transform;

        // スクリプトを取得する
        var component = go.GetComponent<SearchEnemy_Script>();
        // リストに追加する
        m_list.DrumonList.Add(component);

        // 出現数を加算する
        m_currentNumberOfDrumons++;
    }
}
