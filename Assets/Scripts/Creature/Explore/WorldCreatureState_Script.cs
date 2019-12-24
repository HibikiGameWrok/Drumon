/*----------------------------------------------------------*/
//  file:      WorldCreatureState_Script.cs                     |
//				 											                    |
//  brief:    ワールド上のDrumonの状態遷移のスクリプト  |
//															                    |
//  date:	2019.11.29									            |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 探索でのドラモンの状態遷移クラス
public abstract class WorldCreatureState_Script
{
    // メンバ変数
    protected NavMeshController_Script m_controller;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public abstract void Initialize(NavMeshController_Script controller);


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
