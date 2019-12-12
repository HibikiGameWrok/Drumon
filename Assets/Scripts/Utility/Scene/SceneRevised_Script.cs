using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRevised_Script : IScene_Script
{
    public override void Dispose()
    {
       
    }

    public override bool Execute()
    {

        return true;
    }

    public override void Initialize(SceneManager_Script manager)
    {
        m_manager = manager;

        m_manager.Audio.PlayBGM(BfxType.bgm_Search);
    }
}
