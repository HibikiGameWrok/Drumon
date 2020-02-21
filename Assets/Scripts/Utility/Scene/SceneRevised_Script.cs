/*----------------------------------------------------------*/
//  file:      SceneRevised_Script.cs                              |
//				 											                    |
//  brief:    探索シーンのスクリプト			                    |
//															                    |
//  date:	2019.12.13									            |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// 探索シーンクラス
public class SceneRevised_Script : IScene_Script
{
    // コンポーネントを取得する変数
    private AddDrumonList_Script m_drumonList;
    private PanelUI_Fade_Script m_panelUIFade_Script = null;
    private bool m_isTransitionBattle = false;


    /// <summary>
    /// 終了処理
    /// </summary>
    public override void Dispose()
    {
        // BGMを止める
        m_manager.Audio.AttachBGMSource.Stop();

        m_isTransitionBattle = false;
    }


    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns></returns>
    public override SceneID Execute()
    {
        // 対象オブジェクトを探す
        FindSearchEnemy();
        IsActivePlayer();


        // バトルシーンへ
        if (m_isTransitionBattle == true)
        {
            // フェードを起動
            m_panelUIFade_Script.IsFadeOut = true;

            // 非同期処理のSceneロード
            TransitionManager_Script.StartTransition(m_manager.Battle.Name, UnityEngine.SceneManagement.LoadSceneMode.Additive);

            // プレイヤーを非アクティブにしておく
            if (m_manager.Player.activeSelf == true)
            {
                m_manager.Player.SetActive(false);
            }

            // フェードし終えたか
            if (m_panelUIFade_Script.IsFadeComp == true)
            {
                return SceneID.SCENE_BATTLE;
            }
        }
        

        // タイトルシーンへ
        if (Input.GetKeyDown(KeyCode.R))
        {
            // 非同期処理のSceneロード
            TransitionManager_Script.StartTransition(m_manager.Title.Name);

            return SceneID.SCENE_TITLE;
        }

        if(AriaOver_Script.Get)
        {
            if(AriaOver_Script.Get.IsOver)
            {
                // フェードを起動
                m_panelUIFade_Script.IsFadeOut = true;

                // フェードし終えたか
                if (m_panelUIFade_Script.IsFadeComp == true)
                {
                    TransitionManager_Script.StartTransition(m_manager.Revised.Name);
                    return SceneID.SCENE_REVISED;
                }
            }
        }

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
        m_manager.Audio.PlayBGM(BfxType.bgm_Search);
        // Findする
        m_manager.Player = GameObject.Find("LocalAvatar");
        m_panelUIFade_Script = GameObject.Find("FadePanel").GetComponent<PanelUI_Fade_Script>();
        for (int i = 0; i < CreatureList_Script.Get.List.DataList.Length; i++)
        {
            if (!CreatureList_Script.Get.List.DataList[i].drumonName.Equals(""))
            {
                CreatureList_Script.Get.List.DataList[i].hp = CreatureList_Script.Get.List.DataList[i].maxHp;
            }
        }
    }


    /// <summary>
    /// 対象オブジェクトを探し出す
    /// </summary>
    private void FindSearchEnemy()
    {      
        // nullならfindする
        if(m_drumonList == null)
            m_drumonList = GameObject.FindObjectOfType<AddDrumonList_Script>();

        // nullなら処理しない
        if (!m_drumonList || m_drumonList.DrumonList.Find(x => x.IsHit == true) == null)
            return;

        // IsHitがtrueならtransitionする
        if (m_drumonList.DrumonList.Find(x => x.IsHit == true).IsHit == true)
            m_isTransitionBattle = true; 
    }

    private void IsActivePlayer()
    {
        if (!m_manager.Player)
            return;
        if (m_manager.Player.activeSelf == true)
            return;
        else
            m_manager.Player.SetActive(true);
    }
}
