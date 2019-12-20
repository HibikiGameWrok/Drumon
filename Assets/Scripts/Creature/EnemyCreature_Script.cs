using UnityEngine;
using System.Text.RegularExpressions;

public class EnemyCreature_Script : MonoBehaviour, ICreature_Script
{
    [SerializeField]
    private EnemyCreature m_enemy = null;
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
    private GameObject m_healProsperityUI = null;
    private HealProsperityUI_Script m_healProsperityUIScript = null;
    // AttackWaitTimeUI
    private GameObject m_waitTime = null;
    private EnemyWaitTimeUI_Script m_enemyWaitTimeUIScript = null;

    // Start is called before the first frame update
    void Start()
    {
        m_data = m_enemy.EnemyCreatureData;

#if UNITY_EDITOR
        m_data.data.hp = m_data.data.maxHp;
#endif

        this.m_timer = 0.0f;

        this.m_atkFlag = false;

        CreatePrefab();

        m_healProsperityUI = GameObject.Find("ESlider");
        m_healProsperityUIScript = m_healProsperityUI.GetComponent<HealProsperityUI_Script>();

        m_waitTime = GameObject.Find("WaitTime");
        m_enemyWaitTimeUIScript = m_waitTime.GetComponent<EnemyWaitTimeUI_Script>();

        m_healProsperityUIScript.MaxPoint = m_data.data.maxHp;
        m_healProsperityUIScript.NowPoint = m_data.data.hp;

        m_enemyWaitTimeUIScript.MaxPoint = m_data.data.waitTime;
        m_enemyWaitTimeUIScript.NowPoint = m_timer;
    }

    public void Execute()
    {
        m_healProsperityUIScript.NowPoint = m_data.data.hp;
        m_enemyWaitTimeUIScript.NowPoint = m_timer;
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
        if (this.m_data.data.hp <= 0 && this.transform.childCount != 0)
        {
            m_anim.SetTrigger("Death");
            for (int i = 0; i < this.transform.childCount; i++)
            {
                GameObject.Destroy(this.transform.GetChild(i).gameObject, m_animState.length);
                if (!this.transform.GetChild(i).gameObject.GetComponent<ScaleController_Script>())
                {
                    this.transform.GetChild(i).gameObject.AddComponent<ScaleController_Script>().EndTime = m_animState.length;
                }
            }
        }
        else if (this.transform.childCount == 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Capture(int hitNum)
    {
        if (this.m_data.data.maxHp - this.m_data.data.hp + hitNum > this.m_data.data.maxHp + 10)
        {
            CreatureList_Script.Get.Add(this);
            for (int i = 0; i < this.transform.childCount; i++)
            {
                GameObject.Destroy(this.transform.GetChild(i).gameObject);
            }
            GameObject obj = Resources.Load("VFX/CatchAnimationManager_" + Regex.Replace(m_data.name, @"[^a-z,A-Z]", "")) as GameObject;

            if (!obj) return;
            obj = Instantiate(obj, this.transform.position + obj.transform.position, this.transform.rotation * obj.transform.rotation);
            obj.transform.parent = this.gameObject.transform;
            CatchAnimation_Script ge = obj.GetComponent<CatchAnimation_Script>();
            //ge.TargetPosition = PlayerPositionGetter.Get.transform.position;
            ge.TargetPosition = Vector3.zero;
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
        obj = Instantiate(obj, this.transform.position + obj.transform.position, this.transform.rotation * obj.transform.rotation);
        obj.transform.parent = this.gameObject.transform;

        this.m_anim = obj.GetComponent<Animator>();
        this.m_animState = this.m_anim.GetCurrentAnimatorStateInfo(0);
        this.m_timer = 0.0f;
    }
}
