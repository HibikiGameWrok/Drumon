using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonBase_Script<T> : MonoBehaviour where T : MonoBehaviour
{
    // メンバ変数
    // インスタンス
    private static T m_instance;

    public static T Get
    {
        get
        {
            if(m_instance == null)
            {
                Type t = typeof(T);

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

    protected virtual void Awake()
    {
        // 他のゲームオブジェクトにアタッチされているか調べる
        CheckInstance();
    }

    protected bool CheckInstance()
    {
        if(m_instance == null)
        {
            m_instance = this as T;

            return true;
        }
        else if(m_instance == this)
        {
            return true;
        }
        Destroy(this);

        return false;
    }
}
