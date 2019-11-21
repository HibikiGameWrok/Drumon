using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public class ButtonAction : MonoBehaviour
{
    private async void Start()
    {
        await TransitionManager_Script.OnTransitionFinishedAsync();

        Observable.EveryUpdate()
            .Where(_ => OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            .Subscribe(_ =>
            {
                TransitionManager_Script.StartTransition("");
            });
       
    }
}
