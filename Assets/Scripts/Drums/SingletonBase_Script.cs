/*----------------------------------------------------------*/
//  file:      SingletonBase_Script.cs							    |
//				 															    |
//  brief:    シングルトンベースのスクリプト		            |
//              Singleton Base Class 				                    |
//																				|
//  date:	2019.10.11											    |
//																				|
//  author: Renya Fukuyama										|
/*----------------------------------------------------------*/

// using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// シングルトンベースクラスの定義
public abstract class SingletonBase_Script<T> : MonoBehaviour where T : MonoBehaviour
{
    // メンバ変数
    // インスタンス
    private static T m_instance;
   

    /// <summary>
    /// インスタンスを取得する
    /// </summary>
    public static T Get
    {
        get
        {
            if(m_instance == null)
            {
                System.Type t = typeof(T);

                m_instance = (T)FindObjectOfType(t);

                // エラー
                if (m_instance == null)
                {
                    Debug.LogError(t + "をアタッチしているGameObjectはありません。");
                }
            }

            return m_instance;
        }
    }


    /// <summary>
    /// Awake関数
    /// </summary>
    protected virtual void Awake()
    {
        // 他のゲームオブジェクトにアタッチされているか調べる
        // アタッチされている場合は破棄する
        CheckInstance();
    }


    /// <summary>
    /// 既にインスタンスがあるか確認する
    /// </summary>
    /// <returns>true=存在しない false=存在する</returns>
    protected bool CheckInstance()
    {
        // インスタンスがnullなら生成する
        if(m_instance == null)
        {
            m_instance = this as T;

            return true;
        }
        // インスタンスが同じならreturnする
        else if(m_instance == this)
        {
            return true;
        }
        // それ以外は破棄する
        Destroy(this);

        return false;
    }
}
