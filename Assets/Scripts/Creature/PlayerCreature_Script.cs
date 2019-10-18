using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCreature_Script : MonoBehaviour, ICreature_Script
{
    [SerializeField, Range(1,100)]
    private int HEAL_RATE;

    [SerializeField]
    private CharactorData m_data;

    private int m_hp;
    private int m_atk;
    private int m_def;
    private CharactorData.ELEM m_elem;

    private float m_timer;

    private ICreature_Script m_target;

    // Start is called before the first frame update
    void Start()
    {
        this.m_hp = m_data.Hp;
        this.m_atk = m_data.Atk;
        this.m_def = m_data.Def;
        this.m_elem = m_data.Elem;

        this.m_timer = 0.0f;

        this.m_target = null;
    }

    public void Execute()
    {
        this.CountTimer();
        if(this.m_target != null && this.m_timer >= 5.0f)
        {
            this.Attack(1);
            this.m_timer = 0.0f;
        }
    }

    public void CountTimer()
    {
        this.m_timer += Time.deltaTime;
    }

    public void Attack(int rate)
    {
        int damage = (this.m_atk * rate / 2) - (this.m_target.GetData().Def / 4);

        this.m_target.Damage(damage);
    }

    public void Damage(int damage)
    {
        this.m_hp -= damage;
    }

    public void Heal()
    {
        this.m_hp = m_data.Hp / 100 * HEAL_RATE;
    }

    public CharactorData GetData()
    {
        return this.m_data;
    }

    public void SetTarget(ICreature_Script target)
    {
        this.m_target = target;
    }
}
