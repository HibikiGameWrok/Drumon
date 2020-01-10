/*----------------------------------------------------------*/
//  file:      IScene_Script.cs                                        |
//				 											                    |
//  brief:    SceneのInterfaceスクリプト		                    |
//															                    |
//  date:	2019.12.12									            |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum SceneID
    {
        CONTINUE,
        SCENE_TITLE,
        SCENE_REVISED,
        SCENE_BATTLE,
        SCENE_RESULT,
        SCENE_TUTORIAL,
        SCENE_ENGING,
    }
// SceneのInterface
public abstract class IScene_Script
{
    // 親オブジェクト
    protected SceneManager_Script m_manager;
    // シーンの名前
    protected string m_name;

    public string Name
    {
        get { return m_name; }
        set { m_name = value; }
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    public abstract void Initialize(SceneManager_Script manager);


    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns>true=継続する false=シーン変更</returns>
    public abstract SceneID Execute();


    /// <summary>
    /// 終了処理
    /// </summary>
    public abstract void Dispose();
}
