using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTitle_Script : IScene_Script
{

    public override void Dispose()
    {
        
    }

    public override bool Execute()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            m_manager.Audio.PlaySE(SfxType.taiko);
            return false;
        }

        return true;
    }

    public override void Initialize(SceneManager_Script manager)
    {
        m_manager = manager;

        m_manager.Audio.PlayBGM(BfxType.bgm_Title);
    }
}
