/*----------------------------------------------------------*/
//  file:      AudioManager_Script.cs                             |
//				 											                    |
//  brief:    Audio関係のスクリプト				                    |
//															                    |
//  date:	2019.11.12									            |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;


// Audio関連のクラス
public class AudioManager_Script : MonoBehaviour
{
    // ボリューム保存用のkeyとデフォルト値
    private const string BGM_VOLUME_KEY = "BGM_VOLUME_KEY";
    private const string SE_VOLUME_KEY = "SE_VOLUME_KEY";
    private const float BGM_VOLUME_DEFAULT = 1.0f;
    private const float SE_VOLUME_DEFAULT = 1.0f;

    // BGMがフェードするのにかかる時間
    public const float BGM_FADE_SPEED_RATE_HIGH = 0.9f;
    public const float BGM_FADE_SPEED_RATE_LOW = 0.3f;
    private float m_bgmFadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH;

    // 次に流すBGM,SE
    private string m_nextBGMName;
    private string m_nextSEName;

    // BGMをフェードアウト中か
    public ReactiveProperty<bool> m_isFadeOut = new ReactiveProperty<bool>();

    // BGM用、SE用に分けてオーディオソースを持つ
    public AudioSource AttachBGMSource, AttachSESource;

    // 全Audio保持
    [SerializeField]
    private Dictionary<string, AudioClip> m_bgmDic, m_seDic;


    private void Awake()
    {
        // リソースフォルダから全SE＆BGMのファイルを読み込みセット
        m_bgmDic = new Dictionary<string, AudioClip>();
        m_seDic = new Dictionary<string, AudioClip>();

        object[] bgmList = UnityEngine.Resources.LoadAll("Musics/BGM");
        object[] seList = UnityEngine.Resources.LoadAll("Musics/SE");

        foreach(AudioClip bgm in bgmList)
        {
            m_bgmDic[bgm.name] = bgm;
        }

        foreach(AudioClip se in seList)
        {
            m_seDic[se.name] = se;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // 音量を設定する
        AttachBGMSource.volume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFAULT);
        AttachSESource.volume = PlayerPrefs.GetFloat(SE_VOLUME_KEY, SE_VOLUME_DEFAULT);

        m_isFadeOut.SetValueAndForceNotify(false);

        // 何かしたときに再生する
        m_isFadeOut
            .Subscribe(x =>
            {
                if (!x)
                    return;

                // 徐々にボリュームを下げていき、0になったら次の曲を流す
                AttachBGMSource.volume -= Time.deltaTime * m_bgmFadeSpeedRate;
               
                if(AttachBGMSource.volume <= 0)
                {
                    AttachBGMSource.Stop();
                    AttachBGMSource.volume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFAULT);
                    m_isFadeOut.SetValueAndForceNotify(false);

                    if(!string.IsNullOrEmpty(m_nextBGMName))
                    {
                        PlayBGM(m_nextBGMName);
                    }
                }
            }).AddTo(this.gameObject);
    }

    /// <summary>
    /// 指定したファイル名のSEを流す
    /// </summary>
    /// <param name="seName">SE名</param>
    /// <param name="delay">再生までの間隔をあける</param>
    public void PlaySE(string seName, float delay = 0.0f)
    {
        // SEが存在しないなら処理しない
        if(!m_seDic.ContainsKey(seName))
        { 
            Debug.Log(seName + "というSEはありません");
            return;
        }

        m_nextSEName = seName;
        Invoke("DelayPlaySE", delay);
    }
    public void PlaySE(SfxType audio , float delay = 0.0f)
    {
        PlaySE(audio.ToString(), delay);
    }


    private void DelayPlaySE()
    {
        AttachSESource.PlayOneShot(m_seDic[m_nextSEName] as AudioClip);
    }


    /// <summary>
    /// 指定したファイル名のBGMを流す
    /// 既に流れているなら前の曲をフェードアウトさせえてから
    /// </summary>
    /// <param name="bgmName">BGM名</param>
    /// <param name="fadeSpeedRate">割合でフェードアウトするスピード</param>
    public void PlayBGM(string bgmName, float fadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH)
    {
        // BGMが存在しないなら処理しない
        if(!m_bgmDic.ContainsKey(bgmName))
        {
            Debug.Log(bgmName + "というBGMはありません");
            return;
        }

        // 現在BGMが流れていないならそのまま流す
        if(!AttachBGMSource.isPlaying)
        {
            m_nextBGMName = "";
            AttachBGMSource.clip = m_bgmDic[bgmName] as AudioClip;
            AttachBGMSource.Play();
        }
        // 違うBGMが流れているならフェードアウトさせてから次を流す
        else if(AttachBGMSource.clip.name != bgmName)
        {
            m_nextBGMName = bgmName;
            FadeOutBGM(fadeSpeedRate);
        }
    }

    public void PlayBGM(BfxType audio , float fadeSpeedRate = 0.0f)
    {
        PlayBGM(audio.ToString(), fadeSpeedRate);
    }

    /// <summary>
    /// 現在流れている曲をフェードアウトさせる
    /// </summary>
    /// <param name="fadeSpeedRate">フェードアウトするスピード</param>
    private void FadeOutBGM(float fadeSpeedRate)
    {
        m_bgmFadeSpeedRate = fadeSpeedRate;
        m_isFadeOut.SetValueAndForceNotify(true);
    }


    /// <summary>
    /// BGMとSEのボリュームを別々に変更、保存
    /// </summary>
    /// <param name="BGMVolume">BGMボリューム</param>
    /// <param name="SEVolume">SEボリューム</param>
    public void ChangeVolume(float BGMVolume, float SEVolume)
    {
        AttachBGMSource.volume = BGMVolume;
        AttachSESource.volume = SEVolume;

        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, BGMVolume);
        PlayerPrefs.SetFloat(SE_VOLUME_KEY, SEVolume);
    }
}
