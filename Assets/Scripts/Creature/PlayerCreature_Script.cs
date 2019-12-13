using UnityEngine;
using System.Text.RegularExpressions;

public class PlayerCreature_Script : MonoBehaviour, ICreature_Script
{
    [SerializeField, Range(1,100)]
    private int HEAL_RATE = 0;

    private CreatureData m_data = null;

    private Animator m_anim = null;
    private AnimatorStateInfo m_animState;

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
    private GameObject m_TimerObject = null;
    private AccelerationTime_Script m_accelerationTimeScript = null; 

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

    private string m_abiltyName;
    public string AbiltyName
    {
        set { m_abiltyName = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        this.m_timer = 0.0f;

        m_rate = 0;

        this.m_atkFlag = false;

        m_attackRecipe = FindObjectOfType<AttackRecipeManeger_Script>();
        m_healProsperityUI = GameObject.Find("PSlider");
        m_healProsperityUIScript = m_healProsperityUI.GetComponent<HealProsperityUI_Script>();
        m_TimerObject = GameObject.Find("Timer");
        m_accelerationTimeScript = m_TimerObject.GetComponent<AccelerationTime_Script>();

        PlayerBox_Script box = CreatureList_Script.Get.List;
        for (int i = 0; i < box.DataList.Length; i++)
        {
            if (box.DataList[i] != null && box.DataList[i].data.hp != 0)
            {
                ChangeData(box.DataList[i]);
                break;
            }
        }

        m_attackRecipe.CSVLoadFile(this);

        m_healProsperityUIScript.MaxPoint = m_data.data.maxHp;
        m_healProsperityUIScript.NowPoint = m_data.data.hp;

        m_accelerationTimeScript.MaxTimer = m_data.data.waitTime;
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
        int damage = (this.m_data.data.atk * this.m_rate) - (this.m_target.GetData().data.def);
        float weak = WeakChecker_Script.WeakCheck(this.m_data.data.elem, this.m_target.GetData().data.elem);
        VFXCreater_Script.CreateEffect(m_abiltyName, this.transform);
        damage = (int)(damage * weak);
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
        if (this.m_data.data.hp > this.m_data.data.maxHp) this.m_data.data.hp = this.m_data.data.maxHp;
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

            m_healProsperityUIScript.MaxPoint = m_data.data.maxHp;
            m_healProsperityUIScript.NowPoint = m_data.data.hp;
            m_accelerationTimeScript.MaxTimer = m_data.data.waitTime;
        }
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
            PlayerBox_Script box = CreatureList_Script.Get.List;
            for(int i = 0;i<box.DataList.Length;i++)
            {
                if(box.DataList[i] != null && box.DataList[i].data.hp != 0)
                {
                    ChangeData(box.DataList[i]);
                    return;
                }
            }
            Destroy(this.gameObject);
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

        GameObject obj = Resources.Load("InsPrefab/PlayerCreaturePrefab/" + Regex.Replace(m_data.name, @"[^a-z,A-Z]", "")) as GameObject;

        if(!obj) obj = Resources.Load("InsPrefab/PlayerCreaturePrefab/Wolf_fbx") as GameObject;
        obj = Instantiate(obj, this.transform.position + obj.transform.position, this.transform.rotation * obj.transform.rotation);
        obj.transform.parent = this.gameObject.transform;

        this.m_anim = obj.GetComponent<Animator>();
        this.m_animState = this.m_anim.GetCurrentAnimatorStateInfo(0);
        this.m_timer = 0.0f;
    }
}
