﻿using UnityEngine;
using System.Text.RegularExpressions;

public class PlayerCreature_Script : MonoBehaviour, ICreature_Script
{
    [SerializeField, Range(1,100)]
    private int HEAL_RATE = 0;

    [SerializeField]
    private CreatureData m_data = null;

    [SerializeField]
    private string m_name = null;

    private Animator m_anim = null;
    private AnimatorStateInfo m_animState;
    private float m_length;

    public string Name
    {
        get { return this.m_data.name; }
    }

    public int HP
    {
        get { return this.m_data.data.hp; }
    }

    private float m_timer;

    private GameObject m_healProsperityUI = null;
    private HealProsperityUI_Script m_healProsperityUIScript = null;

    public float Timer
    {
        get { return this.m_timer; }
    }

    private ICreature_Script m_target = null;

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
        this.m_timer = 0.0f;

        m_rate = 0;

        this.m_atkFlag = false;

        CreatePrefab();

        m_attackRecipe = FindObjectOfType<AttackRecipeManeger_Script>();
        m_attackRecipe.CSVLoadFile(this);

        m_healProsperityUI = GameObject.Find("PSlider");
        m_healProsperityUIScript = m_healProsperityUI.GetComponent<HealProsperityUI_Script>();

        m_healProsperityUIScript.MaxPoint = m_data.data.hp;
    }

    public void Execute()
    {
        //this.CountTimer();
        m_healProsperityUIScript.NowPoint = m_data.data.hp;
        if (this.m_rate != 0)
        {
            this.m_atkFlag = true;
        }
        this.Dead();
    }

    public void CountTimer()
    {
        this.m_timer += Time.deltaTime;
    }

    public void Attack()
    {
        int damage = (this.m_data.data.atk * this.m_rate / 2) - (this.m_target.GetData().data.def / 4);
        
        this.m_target.Damage(damage);
        this.m_rate = 0;
        this.m_atkFlag = false;

        m_anim.SetTrigger("Attack");
    }

    public void Damage(int damage)
    {
        this.m_data.data.hp -= damage;
        GetComponent<ParticleSystem>().Play();
        m_anim.SetTrigger("Damage");
        if (this.m_data.data.hp < 0) this.m_data.data.hp = 0;
    }

    public void Heal()
    {
        this.m_data.data.hp += this.HEAL_RATE;
        //if (this.m_data.Hp > this.m_data.Hp) this.m_data.Hp = this.m_data.Hp;
    }

    public CreatureData GetData()
    {
        return this.m_data;
    }

    public void ChangeData(CreatureData data)
    {
        if(m_data != data)
        {
            m_data = data;
            CreatePrefab();
        }
    }

    public void SetTarget(ICreature_Script target)
    {
        this.m_target = target;
    }

    public void Dead()
    {
        if (this.m_data.data.hp <= 0 && m_length == 0.0f)
        {
            m_anim.SetTrigger("Death");
            m_length = m_animState.length;

            if (m_length == 0.0f) Destroy(this.gameObject);
        }
        else if (m_length != 0.0f)
        {
            this.m_timer += Time.deltaTime;
            if (m_length < this.m_timer)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void CreatePrefab()
    {
        if(this.transform.childCount != 0)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                GameObject.Destroy(this.transform.GetChild(i).gameObject);
            }
        }

        GameObject obj = (GameObject)Resources.Load("InsPrefab/PlayerCreaturePrefab/" + Regex.Replace(m_data.name, @"[^a-z,A-Z]", ""));

        if(!obj) obj = (GameObject)Resources.Load("InsPrefab/PlayerCreaturePrefab/Wolf_fbx");
        obj = Instantiate(obj, this.transform.position, this.transform.rotation);
        obj.transform.parent = this.gameObject.transform;

        this.m_anim = obj.GetComponent<Animator>();
        this.m_animState = this.m_anim.GetCurrentAnimatorStateInfo(0);
        this.m_length = 0.0f;
        this.m_timer = 0.0f;
    }
}
