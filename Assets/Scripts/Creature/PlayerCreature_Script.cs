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

    [SerializeField]
    private string m_name;

    public string Name
    {
        get { return this.m_name; }
    }

    private int m_hp;

    public int HP
    {
        get { return this.m_hp; }
    }

    private int m_atk;
    private int m_def;
    private CharactorData.ELEM m_elem;

    private float m_timer;

    private GameObject m_healProsperityUI;
    private HealProsperityUI_Script m_healProsperityUIScript;

    public float Timer
    {
        get { return this.m_timer; }
    }

    private ICreature_Script m_target;

    private int m_rate;

    public int Rate
    {
        get { return m_rate; }
        set { m_rate = value; }
    }

    private bool m_atkFlag;

    public bool AtkFlag
    {
        get { return m_atkFlag; }
    }

    private AttackRecipeManeger_Script m_attackRecipe;

    // Start is called before the first frame update
    void Start()
    {
        this.m_hp = m_data.Hp;
        this.m_atk = m_data.Atk;
        this.m_def = m_data.Def;
        this.m_elem = m_data.Elem;

        this.m_timer = 0.0f;

        this.m_target = null;

        m_rate = 0;

        this.m_atkFlag = false;

        m_attackRecipe = FindObjectOfType<AttackRecipeManeger_Script>();
        m_attackRecipe.CSVLoadFile(this);

        m_healProsperityUI = GameObject.Find("PSlider");
        m_healProsperityUIScript = m_healProsperityUI.GetComponent<HealProsperityUI_Script>();
    }

    public void Execute()
    {
        this.CountTimer();
        m_healProsperityUIScript.NowPoint = m_hp;
        if (this.m_rate != 0)
        {
            this.m_atkFlag = true;
        }
    }

    public void CountTimer()
    {
        this.m_timer += Time.deltaTime;
    }

    public void Attack()
    {
        int damage = (this.m_atk * this.m_rate / 2) - (this.m_target.GetData().Def / 4);
        
        this.m_target.Damage(damage);
        this.m_timer = 0.0f;
        this.m_rate = 0;
        this.m_atkFlag = false;
    }

    public void Damage(int damage)
    {
        this.m_hp -= damage;
        GetComponent<ParticleSystem>().Play();
        if (this.m_hp < 0) this.m_hp = 0;
        this.Dead();
    }

    public void Heal()
    {
        this.m_hp += this.m_data.Hp / 100 * this.HEAL_RATE;
        if (this.m_hp > this.m_data.Hp) this.m_hp = this.m_data.Hp;
    }

    public CharactorData GetData()
    {
        return this.m_data;
    }

    public void ChangeData(CharactorData data)
    {
        if(m_data != data)
        {
            m_data = data;

            this.m_hp = m_data.Hp;
            this.m_atk = m_data.Atk;
            this.m_def = m_data.Def;
            this.m_elem = m_data.Elem;
        }
    }

    public void SetTarget(ICreature_Script target)
    {
        this.m_target = target;
    }

    public void Dead()
    {
        if (this.m_hp <= 0) Destroy(this.gameObject);
    }
}
