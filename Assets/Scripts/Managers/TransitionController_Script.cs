/*----------------------------------------------------------*/
//  file:      TransitionController_Script.cs                      |
//				 											                    |
//  brief:    トランジションコントローラーのスクリプト	    |
//               				                                                |
//															                    |
//  date:	2019.11.11									            |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// トランジションコントローラークラスの定義
public class TransitionController_Script : MonoBehaviour
{
    [SerializeField]
    private Image m_coverImage;
    [SerializeField]
    private float m_transitionSeconds;

    private readonly ReactiveProperty<bool> m_isTransferring = new ReactiveProperty<bool>(false);

    public IObservable<Unit> OnTransitionFinished
    {
        get
        {
            // シーン遷移をしていないなら、即イベント発行
            if (!m_isTransferring.Value)
                return Observable.Return(Unit.Default);

            // シーン遷移中なら終わったときにイベント発行
            return m_isTransferring.FirstOrDefault(x => !x).AsUnitObservable();
        }
    }


    /// <summary>
    /// シーン遷移を開始する
    /// </summary>
    /// <param name="nextSceneName">次のシーン名</param>
    public void TransitionStart(string nextSceneName)
    {
        // 既にシーン遷移中なら何もしない
        if (m_isTransferring.Value)
            return;

        m_isTransferring.Value = true;

        StartCoroutine(TransitionCoroutine(nextSceneName));
    }
    public void TransitionStart(string nextSceneName,LoadSceneMode mode = LoadSceneMode.Single)
    {
        // 既にシーン遷移中なら何もしない
        if (m_isTransferring.Value)
            return;

        m_isTransferring.Value = true;

        StartCoroutine(TransitionCoroutine(nextSceneName,mode));
    }

    public void UnloadStart(string unloadSceneName)
    {
        // 既にアンロード中なら何もしない
        if (m_isTransferring.Value)
            return;

        m_isTransferring.Value = true;

        StartCoroutine(UnloadCoroutine(unloadSceneName));
    }


    private IEnumerator TransitionCoroutine(string nextSceneName,LoadSceneMode mode = LoadSceneMode.Single)
    {
        var time = m_transitionSeconds;

        // 画面のクリック
        m_coverImage.raycastTarget = true;

        // 画面を徐々に白く
        while(time > 0)
        {
            time -= Time.deltaTime;
            m_coverImage.color = OverrideColorAlpha(m_coverImage.color, 1.0f - time / m_transitionSeconds);
            yield return null;            
        }

        // 完全に白くする
        m_coverImage.color = OverrideColorAlpha(m_coverImage.color, 1.0f);

        // 画面が隠し終わったらシーン遷移する
        yield return SceneManager.LoadSceneAsync(nextSceneName,mode);

        // 徐々に画面を戻す
        time = m_transitionSeconds;

        while(time >0)
        {
            time -= Time.deltaTime;
            m_coverImage.color = OverrideColorAlpha(m_coverImage.color, time / m_transitionSeconds);
            yield return null;
        }

        // クリックイベントのブロック解除
        m_coverImage.raycastTarget = false;
        m_coverImage.color = OverrideColorAlpha(m_coverImage.color, 0.0f);

        // シーン遷移完了
        m_isTransferring.Value = false;
    }


    /// <summary>
    /// シーンのアンロード
    /// </summary>
    /// <param name="unloadSceneName"></param>
    /// <returns></returns>
    private IEnumerator UnloadCoroutine(string unloadSceneName)
    {
        var time = m_transitionSeconds;

        // 画面のクリック
        m_coverImage.raycastTarget = true;

        // 画面を徐々に白く
        while (time > 0)
        {
            time -= Time.deltaTime;
            m_coverImage.color = OverrideColorAlpha(m_coverImage.color, 1.0f - time / m_transitionSeconds);
            yield return null;
        }

        // 完全に白くする
        m_coverImage.color = OverrideColorAlpha(m_coverImage.color, 1.0f);

        // 画面が隠し終わったらシーンをアンロードする
        yield return SceneManager.UnloadSceneAsync(unloadSceneName);

        // 徐々に画面を戻す
        time = m_transitionSeconds;

        while (time > 0)
        {
            time -= Time.deltaTime;
            m_coverImage.color = OverrideColorAlpha(m_coverImage.color, time / m_transitionSeconds);
            yield return null;
        }

        // クリックイベントのブロック解除
        m_coverImage.raycastTarget = false;
        m_coverImage.color = OverrideColorAlpha(m_coverImage.color, 0.0f);

        // シーン遷移完了
        m_isTransferring.Value = false;
    }

    private Color OverrideColorAlpha(Color c, float a)
    {
        return new Color(c.r, c.g, c.b, a);
    }
}
