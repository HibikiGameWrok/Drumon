using UnityEngine;
using System.Text.RegularExpressions;

public class PlayerCreature_Script : MonoBehaviour, ICreature_Script
{
    [SerializeField]
    private float m_expRate = 0.0f;

    [SerializeField]
    private int m_expPoint = 0;

    [SerializeField]
    private int[] m_upPoint = null;

    private CreatureData m_data = null;

    private Animator m_anim = null;
    private AnimatorStateInfo m_animState;

    public string Name
    {
        get { return this.m_data.drumonName; }
    }

    public float WaitTime
    {
        get { return this.m_data.waitTime; }
    }

    private GameObject m_healProsperityUI = null;
    private HealProsperityUI_Script m_healProsperityUIScript = null;
    private GameObject m_TimerObject = null;
    private AccelerationTime_Script m_accelerationTimeScript = null;
    private GameObject m_levelUI = null;
    private LevelTextUI_Script m_levelTextUIScript = null;

    private float m_timer;

    private ICreature_Script m_target = null;

    private int m_rate;
    public int Rate
    {
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
        m_levelUI = GameObject.Find("PLVText");
        m_levelTextUIScript = m_levelUI.GetComponent<LevelTextUI_Script>();


        PlayerBox_Script box = CreatureList_Script.Get.List;
        for (int i = 0; i < box.DataList.Length; i++)
        {
            if (!box.DataList[i].drumonName.Equals("") && box.DataList[i].hp != 0)
            {
                ChangeData(box.DataList[i]);
                break;
            }
        }

#if UNITY_EDITOR
        m_data.hp = m_data.maxHp;
#endif

        m_healProsperityUIScript.MaxPoint = m_data.maxHp;
        m_healProsperityUIScript.NowPoint = m_data.hp;
        m_levelTextUIScript.NowLevel = m_data.level;
        m_accelerationTimeScript.MaxTimer = m_data.waitTime;
    }

    public void Execute()
    {
        //this.CountTimer();
        m_healProsperityUIScript.NowPoint = m_data.hp;
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
        int damage = (int)(this.m_data.atk * (this.m_rate / 100.0f)) - (this.m_target.GetData().def);
        float weak = WeakChecker_Script.WeakCheck(this.m_data.elem, this.m_target.GetData().elem);
        VFXCreater_Script.CreateEffect(m_abiltyName, this.transform);
        damage = (int)(damage * weak);
        this.m_target.Damage(damage);
        this.m_rate = 0;
        this.m_atkFlag = false;

        m_anim.SetTrigger("Attack");
    }

    public void Damage(int damage)
    {
        this.m_data.hp -= damage;
        m_anim.SetTrigger("Damage");
        if (this.m_data.hp < 0) this.m_data.hp = 0;
    }

    public void Heal()
    {
        // 回復SEを再生する
        AudioManager_Script.Get.PlaySE(SfxType.Heal);

        this.m_data.hp += this.m_rate;
        if (this.m_data.hp > this.m_data.maxHp) this.m_data.hp = this.m_data.maxHp;
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
            m_attackRecipe.CSVSetting(m_data.drumonName);
            m_healProsperityUIScript.MaxPoint = m_data.maxHp;
            m_healProsperityUIScript.NowPoint = m_data.hp;
            m_levelTextUIScript.NowLevel = m_data.level;
            m_accelerationTimeScript.MaxTimer = m_data.waitTime;
        }
    }

    public void SetTarget(ICreature_Script target)
    {
        this.m_target = target;
    }

    public void Dead()
    {
        if (this.m_data.hp <= 0 && this.transform.childCount != 0)
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
                if(box.DataList[i] != null && box.DataList[i].hp != 0)
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

        GameObject obj = Resources.Load("InsPrefab/PlayerCreaturePrefab/" + Regex.Replace(m_data.drumonName, @"[^a-z,A-Z]", "")) as GameObject;

        if(!obj) obj = Resources.Load("InsPrefab/PlayerCreaturePrefab/Wolf_fbx") as GameObject;
        obj = Instantiate(obj, this.transform.position + obj.transform.position, this.transform.rotation * obj.transform.rotation);
        obj.transform.parent = this.gameObject.transform;

        this.m_anim = obj.GetComponent<Animator>();
        this.m_animState = this.m_anim.GetCurrentAnimatorStateInfo(0);
        this.m_timer = 0.0f;
    }

    public void AddExpPoint()
    {
        PlayerBox_Script box = CreatureList_Script.Get.List;
        for (int i = 0; i < box.DataList.Length; i++)
        {
            if (box.DataList[i])
            {
                int too = box.DataList[i].exp -= m_expPoint;
                CheckLevelUp(too, box.DataList[i]);
            }
        }
    }

    private void CheckLevelUp(int too, CreatureData data)
    {
        if (too <= 0 && data.level <= 10)
        {
            LevelUp(this.m_upPoint[Random.Range(0, 2)], data);
            data.exp = (int)(m_expPoint * m_expRate);
            if (too == 0) return;
            too = data.exp + too;
            CheckLevelUp(too, data);
        }
    }

    private void LevelUp(int num, CreatureData data)
    {
        data.level += 1;
        data.hp = data.maxHp;
        m_levelTextUIScript.NowLevel = m_data.level;
        for (int i = 0; i < num; i++)
        {
            int rand = Random.Range(0, 2);
            switch (rand)
            {
                case 0:
                    data.hp += 3;
                    data.maxHp = data.hp;
                    break;
                case 1:
                    data.atk += 2;
                    break;
                case 2:
                    data.def += 2;
                    break;
            }
        }
    }
}
