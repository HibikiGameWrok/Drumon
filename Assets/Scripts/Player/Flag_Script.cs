using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag_Script
{
    private uint m_flag;

    // 初期化処理
    void Start()
    {
        m_flag = 0;
    }

    // フラグを立てる
    public void OnFlag(uint flag)
    {
        m_flag |= flag;
    }

    // フラグを伏せる
    public void OffFlag(uint flag)
    {
        m_flag &= ~flag;
    }

    // フラグが立っているか
    public bool IsFlag(uint flag)
    {
        return (m_flag & flag) != 0;
    }
}
