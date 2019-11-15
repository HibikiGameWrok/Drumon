using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDrumonController_Script : MonoBehaviour
{
    public enum ELEM
    {
        FIRE,
        WIND,
        VOID,
        EARTH,
        WATER
    };

    [SerializeField]
    private SO_ElementChecker elementChecker = null;

    [SerializeField]
    private SO_EnemyDrumon_Script data = null;

    [SerializeField]
    private GameEvent AttackNormal_Event = null;
    [SerializeField]
    private GameEvent AttackSpecial_Event = null;
    [SerializeField]
    private GameEvent BoostAttack_Event = null;
    [SerializeField]
    private GameEvent BoostDefense_Event = null;

    [SerializeField]
    private GameObject WaitGaugePrefab = null;
    private GeneralBar_Script WaitGaugeReference = null;

    [SerializeField]
    private GameObject HPGaugePrefab = null;
    private GeneralBar_Script HPGaugeReference = null;

    private int m_hpCurrent = 0;
    private float m_waitTimer = 0.0f; //Time taken for an Action to be taken
    private float m_actionTimer = 0.0f; //Time taken for an Action to finish
    private bool m_actionTaken = false;

    private int m_defBoost = 0; //Current Boosted-Defense
    private int m_atkBoost = 0; //Current Boosted-Attack
    private bool m_chargingSpecialAttack = false;

    private delegate void EnemyDrumonAction();
    private EnemyDrumonAction Action = null;

    //Function : Start
    void Start()
    {
        m_hpCurrent = data.HP;

        WaitGaugeReference = Instantiate(WaitGaugePrefab, GameObject.Find("Canvas").transform).GetComponent<GeneralBar_Script>();

        HPGaugeReference = Instantiate(HPGaugePrefab, GameObject.Find("Canvas").transform).GetComponent<GeneralBar_Script>();
        HPGaugeReference.SetWaitGauge((float)m_hpCurrent/(float)data.HP);

    }

    //Function : Update
    void Update()
    {

        if (Action != null)
        {
            m_actionTimer += Time.deltaTime;
            Action();
            return;
        }

        m_waitTimer += Time.deltaTime;
        if (m_waitTimer >= data.WAIT)
        {
            WaitGaugeReference.SetWaitGauge(1);
            m_waitTimer = 0.0f;
            ExecuteTurn();
        }
        else
        {
            WaitGaugeReference.SetWaitGauge(m_waitTimer / data.WAIT);
        }

    }

    //Function : Damage
    //~~~~~~~~~~~~~~~~~~~~~~~~
    //int _incomingdamage | Damage value of Player's attack
    //ELEM _elem | Element of Player's attack
    //~~~~~~~~~~~~~~~~~~~~~~~~
    //Call this function to attack this Enemy. Minimum damage will always be 1.
    public void Damage(int _incomingdamage, ELEM _elem)
    {
        m_hpCurrent -= (int)Mathf.Clamp((float)_incomingdamage * elementChecker.CompareElement((int)data.Elem, (int)_elem) - (data.DEF + m_defBoost), 1, 99999);

        if (m_hpCurrent <= 0)
        {
            m_hpCurrent = 0;

            //Enemy dies

            
        }

        HPGaugeReference.SetWaitGauge((float)m_hpCurrent / (float)data.HP);

    }

    //Function : ExecuteTurn
    //~~~~~~~~~~~~~~~~~~~~~~~~
    //This function is called when the Enemy Wait Gauge is full
    void ExecuteTurn()
    {
        switch(data.Behaviour)
        {
            case SO_EnemyDrumon_Script.BEHAVIOUR.TIMID:
                {
                    int Chance = Random.Range(0, 100);
                    if (Chance < 30 + ((m_hpCurrent < 0.4f * data.HP) ? 40 : 0)) SetNextAction(AttackNormal);
                    else SetNextAction(BoostDefense);
                }
                break;
            case SO_EnemyDrumon_Script.BEHAVIOUR.AGGRESSIVE:
                {
                    if (m_chargingSpecialAttack)
                    {
                        SetNextAction(AttackSpecial);
                        break;
                    }

                    int Chance = Random.Range(0, 100);
                    if (Chance < 30) SetNextAction(AttackNormal);
                    else if (Chance < 30 + 10 + ((m_hpCurrent < 0.4f * data.HP) ? 30 : 0)) SetNextAction(AttackSpecial);
                    else SetNextAction(BoostAttack);
                }
                break;
            default:
                break;
        }
    }

    //Function : AttackNormal
    //~~~~~~~~~~~~~~~~~~~~~~~~
    //This is an Action function, for carrying out a Normal Attack
    void AttackNormal()
    {
        if (m_actionTimer >= data.AnimTimeAtkNormal)
        {
            Action = null;
        }
        else if (!m_actionTaken && m_actionTimer >= 0.5f * data.AnimTimeAtkNormal)
        {
            m_actionTaken = true;

            //Attack Normal
            AttackNormal_Event.Raise();
        }
    }

    //Function : AttackSpecial
    //~~~~~~~~~~~~~~~~~~~~~~~~
    //This is an Action function, for carrying out a Special Attack
    void AttackSpecial()
    {
        if (m_actionTimer >= data.AnimTimeAtkSpecial)
        {
            Action = null;
        }
        else if (!m_actionTaken && m_actionTimer >= 0.5f * data.AnimTimeAtkSpecial)
        {
            m_actionTaken = true;

            if (m_chargingSpecialAttack) //2nd Turn - Attack
            {
                m_chargingSpecialAttack = false;

                //Attack Special
                AttackSpecial_Event.Raise();
            }
            else //1st Turn - Charging up
            {
                m_chargingSpecialAttack = true;
            }
            
        }
    }

    //Function : BoostAttack
    //~~~~~~~~~~~~~~~~~~~~~~~~
    //This is an Action function, for increasing Attack
    void BoostAttack()
    {
        if (m_actionTimer >= data.AnimTimeBoostAtk)
        {
            Action = null;
        }
        else if (!m_actionTaken && m_actionTimer >= 0.5f * data.AnimTimeBoostAtk)
        {
            m_actionTaken = true;
            m_atkBoost += data.AtkBoostAmount;

            BoostAttack_Event.Raise();
        }
    }

    //Function : BoostDefense
    //~~~~~~~~~~~~~~~~~~~~~~~~
    //This is an Action function, for increasing Defense
    void BoostDefense()
    {
        if (m_actionTimer >= data.AnimTimeBoostDef)
        {
            Action = null;
        }
        else if (!m_actionTaken && m_actionTimer >= 0.5f * data.AnimTimeBoostDef)
        {
            m_actionTaken = true;
            m_defBoost += data.DefBoostAmount;

            BoostDefense_Event.Raise();
        }
    }

    //Function : SetNextAction
    //~~~~~~~~~~~~~~~~~~~~~~~~
    //EnemyDrumonAction _action
    //~~~~~~~~~~~~~~~~~~~~~~~~
    //This function sets the Action of the Enemy, and should normally only be called in ExecuteTurn()
    void SetNextAction(EnemyDrumonAction _action)
    {
        Action = _action;
        m_actionTimer = 0;
        m_actionTaken = false;
    }

}
