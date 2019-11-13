using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public class SampleMain : MonoBehaviour
{
    public GameObject[] gameObjectsTohidden;

    [SerializeField]
    private string m_nextSceneName;
    [SerializeField]
    private UnityEngine.UI.Button m_button;

    // Start is called before the first frame update
    private async void Start()
    {
        await TransitionManager_Script.OnTransitionFinishedAsync();

        /**/SceneManager.sceneUnloaded += OnSceneUnloaded;

        m_button.OnClickAsObservable()
            .Subscribe(_ =>
            {
                foreach (GameObject obj in gameObjectsTohidden)
                {
                    obj.SetActive(false);
                }
                TransitionManager_Script.StartTransition(m_nextSceneName,LoadSceneMode.Additive);
            });
    }

    private void OnSceneUnloaded(Scene current)
    {
        Debug.Log("OnSceneUnloaded" + current.name);

        foreach(GameObject obj in gameObjectsTohidden)
        {
            obj.SetActive(true);
        }
    }
}
