using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SO_EnemyDrumon", menuName = "EnemyDrumon Data")]
public class SO_EnemyDrumon_Script : ScriptableObject
{
    public enum ELEM
    {
        FIRE,
        WIND,
        VOID,
        EARTH,
        WATER
    };

    public enum BEHAVIOUR
    {
        TIMID,
        AGGRESSIVE
    };

    [Header("Basic Stats")]

    [SerializeField]
    private int m_hp;
    [SerializeField]
    private int m_atk;
    [SerializeField]
    private int m_def;
    [SerializeField]
    private float m_wait;
    [SerializeField]
    private ELEM m_elem;
    [SerializeField]
    private float m_catchRate; // Base Catch Rate. Can only be caught within 0 ~ 100. 100 is a guaranteed catch, 0 and below means it cannot be caught.
    [SerializeField]
    private BEHAVIOUR m_behaviour;

    public int HP
    {
        get
        {
            return m_hp;
        }
        set
        {
            m_hp = value;
        }
    }
    public int ATK
    {
        get
        {
            return m_atk;
        }
        set
        {
            m_atk = value;
        }
    }
    public int DEF
    {
        get
        {
            return m_def;
        }
        set
        {
            m_def = value;
        }
    }
    public float WAIT
    {
        get
        {
            return m_wait;
        }
        set
        {
            m_wait = value;
        }
    }
    public ELEM Elem
    {
        get
        {
            return m_elem;
        }
        set
        {
            m_elem = value;
        }
    }
    public float CatchRate
    {
        get
        {
            return m_catchRate;
        }
        set
        {
            m_catchRate = value;
        }
    }
    public BEHAVIOUR Behaviour
    {
        get
        {
            return m_behaviour;
        }
        set
        {
            m_behaviour = value;
        }
    }

    [Header("More Stats")]

    [SerializeField]
    private int m_defBoostAmount;
    [SerializeField]
    private int m_atkBoostAmount;
    [SerializeField]
    private float m_animTimeBoostAtk; //Tentative
    [SerializeField]
    private float m_animTimeBoostDef; //Tentative
    [SerializeField]
    private float m_animTimeAtkNormal; //Tentative
    [SerializeField]
    private float m_animTimeAtkSpecial; //Tentative

    public int DefBoostAmount
    {
        get
        {
            return m_defBoostAmount;
        }
        set
        {
            m_defBoostAmount = value;
        }
    }
    public int AtkBoostAmount
    {
        get
        {
            return m_atkBoostAmount;
        }
        set
        {
            m_atkBoostAmount = value;
        }
    }
    public float AnimTimeAtkNormal
    {
        get
        {
            return m_animTimeAtkNormal;
        }
        set
        {
            m_animTimeAtkNormal = value;
        }
    }
    public float AnimTimeAtkSpecial
    {
        get
        {
            return m_animTimeAtkSpecial;
        }
        set
        {
            m_animTimeAtkSpecial = value;
        }
    }
    public float AnimTimeBoostAtk
    {
        get
        {
            return m_animTimeBoostAtk;
        }
        set
        {
            m_animTimeBoostAtk = value;
        }
    }
    public float AnimTimeBoostDef
    {
        get
        {
            return m_animTimeBoostDef;
        }
        set
        {
            m_animTimeBoostDef = value;
        }
    }

}
