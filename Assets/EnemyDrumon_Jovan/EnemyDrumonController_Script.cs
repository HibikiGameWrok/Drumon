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
    private SO_ElementChecker elementChecker;

    [SerializeField]
    private SO_EnemyDrumon_Script data;

    [SerializeField]
    private GameEvent AttackNormal_Event;
    [SerializeField]
    private GameEvent AttackSpecial_Event;
    [SerializeField]
    private GameEvent BoostAttack_Event;
    [SerializeField]
    private GameEvent BoostDefense_Event;

    [SerializeField]
    private Image WaitGaugeReference;

    private int m_hpCurrent;
    private float m_waitTimer = 0.0f; //Time taken for an Action to be taken
    private float m_actionTimer = 0.0f; //Time taken for an Action to finish
    private bool m_actionTaken = false;

    private int m_defBoost = 0; //Current Boosted-Defense
    private int m_atkBoost = 0; //Current Boosted-Attack
    private bool m_chargingSpecialAttack = false;

    private delegate void EnemyDrumonAction();
    private EnemyDrumonAction Action;

    // Start is called before the first frame update
    void Start()
    {
        m_hpCurrent = data.HP;
        WaitGaugeReference.transform.localScale = new Vector3(0, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {

        //Tentative Debug code
        if (Action == null)
            WaitGaugeReference.rectTransform.localScale = new Vector3(m_waitTimer / data.WAIT, 1, 1);
        else
            WaitGaugeReference.rectTransform.localScale = new Vector3(1, 1, 1);

        if (Action != null)
        {
            m_actionTimer += Time.deltaTime;
            Action();
            return;
        }

        m_waitTimer += Time.deltaTime;
        if (m_waitTimer >= data.WAIT)
        {
            m_waitTimer = 0.0f;
            ExecuteTurn();
        }

    }

    public void Damage(int _incomingdamage, ELEM _elem)
    {
        m_hpCurrent -= (int)Mathf.Clamp(_incomingdamage * elementChecker.CompareElement((int)data.Elem, (int)_elem) - (data.DEF + m_defBoost), 1, 99999);

        if (m_hpCurrent <= 0)
        {
            //Enemy dies

        }
    }

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

    void AttackSpecial()
    {
        if (m_actionTimer >= data.AnimTimeAtkSpecial)
        {
            Action = null;
        }
        else if (!m_actionTaken && m_actionTimer >= 0.5f * data.AnimTimeAtkSpecial)
        {
            m_actionTaken = true;

            //Attack Special
            AttackSpecial_Event.Raise();
        }
    }

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

    void SetNextAction(EnemyDrumonAction _action)
    {
        Action = _action;
        m_actionTimer = 0;
        m_actionTaken = false;
    }

}
