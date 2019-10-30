using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureList_Script : SingletonBase_Script<CreatureList_Script>
{
    [SerializeField]
    private PlayerBox_Script m_list;

    public PlayerBox_Script List
    {
        get { return m_list; }
    }
}
