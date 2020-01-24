using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AriaOver_Script : SingletonBase_Script<AriaOver_Script>
{
    private bool m_isOver = false;

    public bool IsOver
    {
        get { return m_isOver; }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            m_isOver = true;
        }
    }
}
