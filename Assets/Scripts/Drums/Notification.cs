using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : SingletonBase_Script<Notification>
{
    private Notification() { }

    public delegate void OnSomeEvent();
    event OnSomeEvent m_onSomeEvent;


    /// <summary>
    /// 通知購読解除
    /// </summary>
    /// <param name="onEvent"></param>
    public void SubscriveSomeEvent(OnSomeEvent onEvent)
    {
        m_onSomeEvent -= onEvent;
    }


    /// <summary>
    /// 通知
    /// </summary>
    public void NotifySomeEvent()
    {
        if(m_onSomeEvent != null)
        {
            m_onSomeEvent();
        }
    }

    public delegate void OnSomeEventWithArgs(string text);
    event OnSomeEventWithArgs m_onSomeEventWithArgs;


    /// <summary>
    /// 通知購読
    /// </summary>
    /// <param name="onEventWithArgs"></param>
    public void SubscriveSomeEventWithArgs(OnSomeEventWithArgs onEventWithArgs)
    {
        m_onSomeEventWithArgs += onEventWithArgs;
    }


    /// <summary>
    /// 通知購読解除
    /// </summary>
    /// <param name="onEventWithArgs"></param>
    public void UnSubscribeSomeEventWithArgs(OnSomeEventWithArgs onEventWithArgs)
    {
        m_onSomeEventWithArgs -= onEventWithArgs;
    }


    /// <summary>
    /// 通知
    /// </summary>
    /// <param name="text"></param>
    public void NotifySomeEventWithArgs(string text)
    {
        if(m_onSomeEventWithArgs != null)
        {
            m_onSomeEventWithArgs(text);
        }
    }
}