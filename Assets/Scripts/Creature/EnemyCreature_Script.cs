using UnityEngine;
using System.Text.RegularExpressions;

public class EnemyCreature_Script : MonoBehaviour, ICreature_Script
{
    [SerializeField]
    private CharactorData m_data = null;

    public int HP
    {
        get { return this.m_data.Hp; }
    }

    private float m_timer;
    private float m_animTimer;

    private ICreature_Script m_target = null;

    private bool m_atkFlag;

    public bool AtkFlag
    {
        get { return m_atkFlag; }
    }

    private Animator m_anim = null;
    private AnimatorStateInfo m_animState;
    private float m_length;

    // HPUI
    private GameObject m_healProsperityUI;
    private HealProsperityUI_Script m_healProsperityUIScript;

    //private GameObject m_enemyHit;
    //private EnemyHit_Script m_enemyHitScript;

    // Start is called before the first frame update
    void Start()
    {
        this.m_timer = 0.0f;
        this.m_animTimer = 0.0f;

        this.m_atkFlag = false;

        //m_enemyHit = GameObject.Find("Enemy");
        //m_enemyHitScript = m_enemyHit.GetComponent<EnemyHit_Script>();

        //this.m_data = m_enemyHitScript.GetData();

        CreatePrefab();

        m_healProsperityUI = GameObject.Find("ESlider");
        m_healProsperityUIScript = m_healProsperityUI.GetComponent<HealProsperityUI_Script>();
    }

    public void Execute()
    {
        m_healProsperityUIScript.NowPoint = m_data.Hp;
        this.CountTimer();
        if (this.m_timer >= 10.0f) this.m_atkFlag = true;

        this.Dead();
    }

    public void CountTimer()
    {
        this.m_timer += Time.deltaTime;
    }

    public void Attack()
    {
        int damage = (this.m_data.Atk / 2) - (this.m_target.GetData().Def / 4);

        this.m_target.Damage(damage);
        this.m_timer = 0.0f;
        this.m_atkFlag = false;

        m_anim.SetTrigger("Attack");
    }

    public void Damage(int damage)
    {
        this.m_data.Hp -= damage;
        GetComponent<ParticleSystem>().Play();
        m_anim.SetTrigger("Damage");
        if (this.m_data.Hp < 0) this.m_data.Hp = 0;
    }

    public void Heal()
    {
        this.m_data.Hp += 10;
    }

    public CharactorData GetData()
    {
        return this.m_data;
    }

    public void SetData(CharactorData data)
    {
        this.m_data = data;
    }

    public void SetTarget(ICreature_Script target)
    {
        this.m_target = target;
    }

    public void Dead()
    {
        if (this.m_data.Hp <= 0 && m_length == 0.0f)
        {
            m_anim.SetTrigger("IsDeath");
            m_length = m_animState.length;

            if (m_length == 0.0f) Destroy(this.gameObject);
        }
        else if(m_length != 0.0f)
        {
            this.m_animTimer += Time.deltaTime;
            if (m_length < this.m_animTimer)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void Capture(int hitNum)
    {
        if (100 - (this.m_data.Hp / 2) + hitNum > 140)
        {
            CreatureList_Script.Get.Add(this);
            Destroy(this.gameObject);
        }
        else
        {
            this.m_atkFlag = true;
        }
    }

    private void CreatePrefab()
    {
        if (this.transform.childCount != 0)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                GameObject.Destroy(this.transform.GetChild(i).gameObject);
            }
        }

        GameObject obj = (GameObject)Resources.Load("InsPrefab/PlayerCreaturePrefab/" + Regex.Replace(m_data.name, @"[^a-z,A-Z]", ""));

        if (!obj) obj = (GameObject)Resources.Load("InsPrefab/PlayerCreaturePrefab/Wolf_fbx");
        obj = Instantiate(obj, this.transform.position, this.transform.rotation);
        obj.transform.parent = this.gameObject.transform;

        this.m_anim = obj.GetComponent<Animator>();
        this.m_animState = this.m_anim.GetCurrentAnimatorStateInfo(0);
        this.m_length = 0.0f;
        this.m_timer = 0.0f;
        this.m_animTimer = 0.0f;
    }
}
