﻿/*----------------------------------------------------------*/
//  file:      GameManager_Script.cs                             |
//				 											                    |
//  brief:    ゲーム全体を管理スクリプト				            |
//                                      				                        |
//															                    |
//  date:	2019.12.11									            |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;


// ゲーム全体を管理するクラス
public class GameManager_Script : SingletonBase_Script<GameManager_Script>
{
    // プレハブを設定する
    [SerializeField]
    private GameObject m_audioIns;
    [SerializeField]
    private GameObject m_bgmIns;
    [SerializeField]
    private GameObject m_seIns;

    // AudioManager
    private GameObject m_audioManager = null;
    // BGMのAudioResource
    private GameObject m_bgmResource = null;
    // SEのAudioResource
    private GameObject m_seResource = null;

    private AudioManager_Script m_audio;

    protected override void Awake()
    {
        // 必要なものを生成する
        m_audioManager = Instantiate(m_audioIns);
        m_bgmResource = Instantiate(m_bgmIns);
        m_seResource = Instantiate(m_seIns);

        // 削除しない
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(m_audioManager);
        DontDestroyOnLoad(m_bgmResource);
        DontDestroyOnLoad(m_seResource);
    }


    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントを取得する
        m_audio = m_audioManager.GetComponent<AudioManager_Script>();
        // アタッチする
        m_audio.AttachBGMSource = m_bgmResource.GetComponent<AudioSource>();
        m_audio.AttachSESource = m_bgmResource.GetComponent<AudioSource>();

        // BGMを流す
        if (SceneManager.GetActiveScene().name == "Revised")
        {
            m_audio.PlayBGM(BfxType.bgm_Search);
        }
        else
        {
            m_audio.PlayBGM(BfxType.bgm_Title);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SceneManager.GetActiveScene().name == "TitleScene")
            {
                m_audio.PlayBGM(BfxType.bgm_Search, 0.5f);
                TransitionManager_Script.StartTransition("Revised");
            }
        }
    }


    /// <summary>
    /// 削除された時の処理
    /// </summary>
    void OnDestroy()
    {
        if (this.gameObject)
        {
            Destroy(m_audioManager);
            Destroy(m_bgmResource);
            Destroy(m_seResource);
            Destroy(this.gameObject);
        }
    }
}
