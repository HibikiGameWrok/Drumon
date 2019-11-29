using UnityEngine;
using System.Text.RegularExpressions;

public class EnemyCreature_Script : MonoBehaviour, ICreature_Script
{
    [SerializeField]
    private CreatureData m_data = null;

    public int HP
    {
        get { return this.m_data.data.hp; }
    }

    private float m_timer;

    private ICreature_Script m_target = null;

    private bool m_atkFlag;

    public bool AtkFlag
    {
        get { return m_atkFlag; }
    }

    private Animator m_anim = null;
    private AnimatorStateInfo m_animState;

    // HPUI
    private GameObject m_healProsperityUI;
    private HealProsperityUI_Script m_healProsperityUIScript;

    //private GameObject m_enemyHit;
    //private EnemyHit_Script m_enemyHitScript;

    // Start is called before the first frame update
    void Start()
    {
        this.m_timer = 0.0f;

        this.m_atkFlag = false;

        CreatePrefab();

        m_healProsperityUI = GameObject.Find("ESlider");
        m_healProsperityUIScript = m_healProsperityUI.GetComponent<HealProsperityUI_Script>();
    }

    public void Execute()
    {
        m_healProsperityUIScript.NowPoint = m_data.data.hp;
        this.CountTimer();
        if (this.m_timer >= m_data.data.waitTime) this.m_atkFlag = true;

        this.Dead();
    }

    public void CountTimer()
    {
        this.m_timer += Time.deltaTime;
    }

    public void Attack()
    {
        int damage = (this.m_data.data.atk) - (this.m_target.GetData().data.def);
        float weak = WeakChecker_Script.WeakCheck(this.m_data.data.elem, this.m_target.GetData().data.elem);
        damage = (int)(damage * weak);
        this.m_target.Damage(damage);
        this.m_timer = 0.0f;
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
        this.m_data.data.hp += 10;
    }

    public CreatureData GetData()
    {
        return this.m_data;
    }

    public void SetData(CreatureData data)
    {
        this.m_data = data;
    }

    public void SetTarget(ICreature_Script target)
    {
        this.m_target = target;
    }

    public void Dead()
    {
        if (this.m_data.data.hp <= 0)
        {
            m_anim.SetTrigger("Death");
            Destroy(this.gameObject, m_animState.length);
        }
    }

    public void Capture(int hitNum)
    {
        if (100 - (this.m_data.data.hp / 2) + hitNum > 140)
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
        GameObject obj = Resources.Load("InsPrefab/PlayerCreaturePrefab/" + Regex.Replace(m_data.name, @"[^a-z,A-Z]", "")) as GameObject;

        if (!obj) obj = Resources.Load("InsPrefab/PlayerCreaturePrefab/Wolf_fbx") as GameObject;
        obj = Instantiate(obj, this.transform.position, this.transform.rotation);
        obj.transform.parent = this.gameObject.transform;

        this.m_anim = obj.GetComponent<Animator>();
        this.m_animState = this.m_anim.GetCurrentAnimatorStateInfo(0);
        this.m_timer = 0.0f;
    }
}
