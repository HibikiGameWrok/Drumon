using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCreatureIdle_Script : WorldCreatureState_Script
{

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initialize()
    {
        
    }

    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns></returns>
    public override bool Execute()
    {

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
