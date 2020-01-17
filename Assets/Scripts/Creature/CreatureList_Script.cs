using UnityEngine;

public class CreatureList_Script : SingletonBase_Script<CreatureList_Script>
{
    [SerializeField]
    private PlayerBox_Script m_list = null;
    private CreatureData m_overData = null;

    public CreatureData OverData
    {
        get { return m_overData; }
    }

    void Start()
    {
#if UNITY_EDITOR
        for (int i = 0; i < m_list.DataList.Length; i++)
        {
            if (m_list.DataList[i].drumonName.Equals(""))
            {
                m_list.DataList[i].hp = m_list.DataList[i].maxHp;
            }
        }
#endif
    }

    public PlayerBox_Script List
    {
        get { return m_list; }
    }

    public void Add(ICreature_Script creature)
    {
        m_overData = null;
        for(int i = 0; i < m_list.DataList.Length; i++)
        {
            if(m_list.DataList[i].drumonName.Equals(""))
            {
                m_list.DataList[i] = creature.GetData();
                break;
            }
        }
        m_overData = creature.GetData();
    }
}
