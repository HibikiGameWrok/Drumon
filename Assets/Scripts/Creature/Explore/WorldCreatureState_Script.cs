


// using
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 探索でのドラモンの状態遷移クラス
public abstract class WorldCreatureState_Script : MonoBehaviour
{
    /// <summary>
    /// 初期化処理
    /// </summary>
    public abstract void Initialize();


    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns>true=継続する false=状態を変更する </returns>
    public abstract bool Execute();


    /// <summary>
    /// 終了処理
    /// </summary>
    public abstract void Dispose();

}
