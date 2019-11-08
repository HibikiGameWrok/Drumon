using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureList_Script : SingletonBase_Script<CreatureList_Script>
{
    [SerializeField]
    private PlayerBox_Script m_list = null;

    public PlayerBox_Script List
    {
        get { return m_list; }
    }

    public void Add(ICreature_Script creature)
    {
        for(int i = 0; i < m_list.DataList.Length; i++)
        {
            if(!m_list.DataList[i])
            {
                m_list.DataList[i] = creature.GetData();
                break;
            }
        }
    }
}
