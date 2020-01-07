/*----------------------------------------------------------*/
//  file:      SceneTitle_Script.cs                                   |
//				 											                    |
//  brief:    タイトルシーンのスクリプト			                |
//															                    |
//  date:	2019.12.13									            |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// タイトルシーンクラス
public class SceneTitle_Script : IScene_Script
{

    /// <summary>
    /// 終了処理
    /// </summary>
    public override void Dispose()
    {
        // BGMを止める
        m_manager.Audio.AttachBGMSource.Stop();
    }


    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns></returns>
    public override SceneID Execute()
    {
        // SpaceキーまたはVRコントローラーのトリガー
        if (Input.GetKeyDown(KeyCode.Space) ||
            OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            // SEを鳴らす
            m_manager.Audio.PlaySE(SfxType.WaterGun);
            // 非同期処理のSceneロード
            TransitionManager_Script.StartTransition(m_manager.Revised.Name);

            return SceneID.SCENE_REVISED;
        }
        else if (Input.GetKeyDown(KeyCode.I))
            m_manager.Audio.PlaySE(SfxType.WaterGun);
            
        // 継続する
        return SceneID.CONTINUE;
    }


    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="manager">親オブジェクト</param>
    public override void Initialize(SceneManager_Script manager)
    {
        // 親オブジェクトを取得する
        m_manager = manager;

        // BGMを再生する
        m_manager.Audio.PlayBGM(BfxType.bgm_Title);
    }
}
