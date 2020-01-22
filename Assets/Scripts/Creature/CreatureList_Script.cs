using UnityEngine;

public class CreatureList_Script : SingletonBase_Script<CreatureList_Script>
{
    [SerializeField]
    private PlayerBox_Script m_list = null;
    private CreatureData m_overData = null;

    public CreatureData OverData
    {
        set { m_overData = value; }
        get { return m_overData; }
    }

    void Start()
    {
#if UNITY_EDITOR
        for (int i = 0; i < m_list.DataList.Length; i++)
        {
            if (!m_list.DataList[i].drumonName.Equals(""))
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
                m_list.DataList[i].drumonName = creature.GetData().drumonName;
                m_list.DataList[i].level = creature.GetData().level;
                m_list.DataList[i].hp = creature.GetData().hp;
                m_list.DataList[i].maxHp = creature.GetData().maxHp;
                m_list.DataList[i].atk = creature.GetData().atk;
                m_list.DataList[i].def = creature.GetData().def;
                m_list.DataList[i].waitTime = creature.GetData().waitTime;
                m_list.DataList[i].elem = creature.GetData().elem;
                m_list.DataList[i].exp = creature.GetData().exp;

                return;
            }
        }
        m_overData = creature.GetData();
    }

    public void Trade(CreatureData data, int num)
    {
        m_list.DataList[num].drumonName = data.drumonName;
        m_list.DataList[num].level = data.level;
        m_list.DataList[num].hp = data.hp;
        m_list.DataList[num].maxHp = data.maxHp;
        m_list.DataList[num].atk = data.atk;
        m_list.DataList[num].def = data.def;
        m_list.DataList[num].waitTime = data.waitTime;
        m_list.DataList[num].elem = data.elem;
        m_list.DataList[num].exp = data.exp;
    }
}
