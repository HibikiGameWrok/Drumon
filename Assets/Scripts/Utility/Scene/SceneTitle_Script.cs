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
using UnityEngine;


// タイトルシーンクラス
public class SceneTitle_Script : IScene_Script
{
    // タイトルドラム
    private TitleDrum_Script m_titleDrum = null;
    private PanelUI_Fade_Script m_panelUIFade_Script = null;
    private bool m_doneFlag = false;

    private bool m_isMain;

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
        if (m_doneFlag == false)
        {
            if (Input.GetKeyDown(KeyCode.D))
                m_isMain = m_isMain.Equals(false) ? true : false;

            // ゲームプレイを選択
            if (m_titleDrum.SelectCount == 0 && m_titleDrum.Decision == true)
            {
                CreatureData data = null;

                for (int i = 0; i < CreatureList_Script.Get.List.DataList.Length; i++)
                {
                    if (CreatureList_Script.Get.List.DataList[i].drumonName.Equals(""))
                    {
                        data = CreatureList_Script.Get.List.DataList[i];
                        break;
                    }
                }

                CreateData_Script.Get.CreateData(data, "Merlion");

                m_titleDrum.Decision = false;
                m_doneFlag = true;

                // SEを鳴らす
                m_manager.Audio.PlaySE(SfxType.WaterGun);
                // 非同期処理のSceneロード
                TransitionManager_Script.StartTransition(m_manager.Revised.Name);

                return SceneID.SCENE_REVISED;
            }
            else if (m_titleDrum.SelectCount == 1 && m_titleDrum.Decision == true)
            {
                m_titleDrum.Decision = false;
                m_doneFlag = true;

                // SEを鳴らす
                m_manager.Audio.PlaySE(SfxType.WaterGun);
                // 非同期処理のSceneロード
                TransitionManager_Script.StartTransition(m_manager.CaptureTutorial.Name);

                return SceneID.SCENE_CAPTURETUTORIAL;
            }

            //// ゲームプレイを選択
            //if (m_titleDrum.Decision)
            //{
            //    if(m_isMain)
            //    {
            //        CreatureData data = null;

            //        for (int i = 0; i < CreatureList_Script.Get.List.DataList.Length; i++)
            //        {
            //            if (CreatureList_Script.Get.List.DataList[i].drumonName.Equals(""))
            //            {
            //                data = CreatureList_Script.Get.List.DataList[i];
            //                break;
            //            }
            //        }

            //        CreateData_Script.Get.CreateData(data, "Merlion");

            //        m_titleDrum.Decision = false;
            //        m_doneFlag = true;

            //        // SEを鳴らす
            //        m_manager.Audio.PlaySE(SfxType.taiko);


            //        // フェードを起動
            //        m_panelUIFade_Script.IsFadeOut = true;
            //    }
            //    else
            //    {
            //        m_titleDrum.Decision = false;
            //        m_doneFlag = true;

            //        // SEを鳴らす
            //        m_manager.Audio.PlaySE(SfxType.taiko);

            //        // フェードを起動
            //        m_panelUIFade_Script.IsFadeOut = true;
            //    }   
            //}
        }

        // フェードし終えたか
        if (m_panelUIFade_Script.IsFadeComp == true)
        {
            if (m_isMain)
            {
                // 非同期処理のSceneロード
                TransitionManager_Script.StartTransition(m_manager.Revised.Name);
                return SceneID.SCENE_REVISED;
            }
            else
            {
                // 非同期処理のSceneロード
                TransitionManager_Script.StartTransition(m_manager.CaptureTutorial.Name);
                return SceneID.SCENE_CAPTURETUTORIAL;
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
        m_manager.Audio.PlayBGM(BfxType.bgm_Title);

        m_panelUIFade_Script = GameObject.Find("FadePanel").GetComponent<PanelUI_Fade_Script>();

        m_titleDrum = GameObject.Find("TitleDrum").GetComponent<TitleDrum_Script>();

        m_doneFlag = false;

        m_isMain = false;

        CreatureList_Script.Get.Reset();
    }
}
