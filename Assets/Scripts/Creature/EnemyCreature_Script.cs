using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreature_Script : MonoBehaviour, ICreature_Script
{
    [SerializeField]
    private CharactorData m_data;

    private int m_hp;

    public int HP
    {
        get { return this.m_hp; }
    }

    private int m_atk;
    private int m_def;
    private CharactorData.ELEM m_elem;

    private float m_timer;

    private ICreature_Script m_target = null;

    private bool m_atkFlag;

    public bool AtkFlag
    {
        get { return m_atkFlag; }
    }

    // HPUI
    private GameObject m_healProsperityUI;
    private HealProsperityUI_Script m_healProsperityUIScript;

    // Start is called before the first frame update
    void Start()
    {
        this.m_hp = m_data.Hp;
        this.m_atk = m_data.Atk;
        this.m_def = m_data.Def;
        this.m_elem = m_data.Elem;

        this.m_timer = 0.0f;

        this.m_atkFlag = false;

        m_healProsperityUI = GameObject.Find("ESlider");
        m_healProsperityUIScript = m_healProsperityUI.GetComponent<HealProsperityUI_Script>();
    }

    public void Execute()
    {
        m_healProsperityUIScript.NowPoint = m_hp;
        this.CountTimer();
        if (this.m_timer >= 5.0f) this.m_atkFlag = true;
    }

    public void CountTimer()
    {
        this.m_timer += Time.deltaTime;
    }

    public void Attack()
    {
        int damage = (this.m_atk / 2) - (this.m_target.GetData().Def / 4);

        this.m_target.Damage(damage);
        this.m_timer = 0.0f;
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
        this.m_hp += m_data.Hp / 100;
    }

    public CharactorData GetData()
    {
        return this.m_data;
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
