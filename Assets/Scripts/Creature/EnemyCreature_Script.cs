using UnityEngine;
using System.Text.RegularExpressions;

public class EnemyCreature_Script : MonoBehaviour, ICreature_Script
{
    [SerializeField]
    private EnemyCreature m_enemy = null;
    private CreatureData m_data = null;

    private CSVDataHolder csvHolder = new CSVDataHolder();
    private int m_lastArts;

    private GameObject m_targetPos = null;

    public int HP
    {
        get { return this.m_data.hp; }
    }

    public CreatureData Data
    {
        get { return m_data; }
    }

    private float m_timer;

    private ICreature_Script m_target = null;

    private bool m_atkFlag;

    public bool AtkFlag
    {
        get { return m_atkFlag; }
    }

    private bool m_otsoFlag;

    public bool OtsoFlag
    {
        get { return m_otsoFlag; }
    }

    private Animator m_anim = null;
    private AnimatorStateInfo m_animState;

    // HPUI
    private GameObject m_healProsperityUI = null;
    private HealProsperityUI_Script m_healProsperityUIScript = null;
    // AttackWaitTimeUI
    private GameObject m_waitTime = null;
    private EnemyWaitTimeUI_Script m_enemyWaitTimeUIScript = null;
    // レベル
    private GameObject m_levelUI = null;
    private LevelTextUI_Script m_levelTextUIScript = null;


    // Start is called before the first frame update
    void Start()
    {
        m_data = m_enemy.EnemyCreatureData;

#if UNITY_EDITOR
        m_data.hp = m_data.maxHp;
#endif
        // CSVの保管クラスに設定
        csvHolder.CSVLoadFile(m_data.drumonName);
        m_lastArts = csvHolder.CSVDatas.Count;

        this.m_timer = 0.0f;

        this.m_atkFlag = false;
        this.m_otsoFlag = false;

        CreatePrefab();

        m_healProsperityUI = GameObject.Find("ESlider");
        m_healProsperityUIScript = m_healProsperityUI.GetComponent<HealProsperityUI_Script>();

        m_waitTime = GameObject.Find("WaitTime");
        m_enemyWaitTimeUIScript = m_waitTime.GetComponent<EnemyWaitTimeUI_Script>();

        m_levelUI = GameObject.Find("ELVText");
        m_levelTextUIScript = m_levelUI.GetComponent<LevelTextUI_Script>();

        m_targetPos = GameObject.Find("PlayerCreature");

        m_healProsperityUIScript.MaxPoint = m_data.maxHp;
        m_healProsperityUIScript.NowPoint = m_data.hp;
        m_levelTextUIScript.NowLevel = m_data.level;
        m_enemyWaitTimeUIScript.MaxPoint = m_data.waitTime;
        m_enemyWaitTimeUIScript.NowPoint = m_timer;

        if (AudioManager_Script.Get)
            // 鳴き声SE
            AudioManager_Script.Get.PlaySE(m_data.drumonName);
    }

    public void Execute()
    {
        m_healProsperityUIScript.NowPoint = m_data.hp;
        m_enemyWaitTimeUIScript.NowPoint = m_timer;
        this.CountTimer();
        if (this.m_timer >= m_data.waitTime) this.m_atkFlag = true;

        this.Dead();
    }

    public void CountTimer()
    {
        this.m_timer += Time.deltaTime;
    }

    public void Attack()
    {
        if (m_data.hp == 0) return;
        SelectArts();
        // 技のレートをクリーチャーに教える
        string matchRate = csvHolder.CSVDatas[m_lastArts][(int)AttackRecipeManeger_Script.Data_Column.ATK_RATE];
        int rate = int.Parse(matchRate);

        int damage = (int)(((m_data.level / 5 + 2) * this.m_data.atk) * (rate / 100.0f)) - this.m_target.GetData().def;
        float weak = WeakChecker_Script.WeakCheck(this.m_data.elem, this.m_target.GetData().elem);
        VFXCreater_Script.CreateEffect(csvHolder.CSVDatas[m_lastArts][(int)AttackRecipeManeger_Script.Data_Column.ATK_NAME], this.transform);
        damage = (int)(damage * weak);
        if (damage <= 0) damage = 1;
        this.m_target.Damage(damage);
        if (weak == 1.5f) DamageUI_Script.CreateWeakUI(m_targetPos.transform, new Vector3(-0.25f, 2.8f, -3f));
        this.m_timer = 0.0f;
        this.m_atkFlag = false;
        
        if(m_anim) m_anim.SetTrigger("Attack");
    }

    public void Damage(int damage)
    {
        this.m_data.hp -= damage;
        m_anim.SetTrigger("Damage");
        DamageUI_Script.CreateDamageUI(this.transform, new Vector3(1.6f, 2.7f, -3f), damage);
        if (this.m_data.hp < 0) this.m_data.hp = 0;
    }

    public void Heal()
    {
        this.m_data.hp += 10;
    }

    public CreatureData GetData()
    {
        return this.m_data;
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
                if (this.transform.GetChild(i).gameObject.tag.Equals("Otso") && !m_otsoFlag)
                    m_otsoFlag = true;
                GameObject.Destroy(this.transform.GetChild(i).gameObject, m_animState.length);
                if (!this.transform.GetChild(i).gameObject.GetComponent<ScaleController_Script>())
                    this.transform.GetChild(i).gameObject.AddComponent<ScaleController_Script>().EndTime = m_animState.length;
            }
        }
        else if (this.transform.childCount == 0) Destroy(this.gameObject);
    }

    public void Capture(int hitNum)
    {
        if (this.m_data.maxHp - this.m_data.hp + hitNum > this.m_data.maxHp + 10)
        {
            CreatureList_Script.Get.Add(this);
            for (int i = 0; i < this.transform.childCount; i++)
            {
                if (this.transform.GetChild(i).gameObject.tag.Equals("Otso") && !m_otsoFlag)
                    m_otsoFlag = true;
                GameObject.Destroy(this.transform.GetChild(i).gameObject);
            }
            GameObject obj = Resources.Load("VFX/CatchAnimationManager_" + Regex.Replace(m_data.drumonName, @"[^a-z,A-Z]", "")) as GameObject;

            if (!obj) return;
            obj = Instantiate(obj, this.transform.position + obj.transform.position, this.transform.rotation * obj.transform.rotation);
            obj.transform.parent = this.gameObject.transform;
            CatchAnimation_Script ge = obj.GetComponent<CatchAnimation_Script>();
            //ge.TargetPosition = PlayerPositionGetter.Get.transform.position;
            ge.TargetPosition = Vector3.zero;
        }
        else this.m_atkFlag = true;
    }

    private void CreatePrefab()
    {
        GameObject obj = Resources.Load("InsPrefab/PlayerCreaturePrefab/" + Regex.Replace(m_data.drumonName, @"[^a-z,A-Z]", "")) as GameObject;

        if (!obj) obj = Resources.Load("InsPrefab/PlayerCreaturePrefab/Wolf_fbx") as GameObject;
        obj = Instantiate(obj, this.transform.position + obj.transform.position, this.transform.rotation * obj.transform.rotation);
        obj.transform.parent = this.gameObject.transform;

        this.m_anim = obj.GetComponent<Animator>();
        this.m_animState = this.m_anim.GetCurrentAnimatorStateInfo(0);
        this.m_timer = 0.0f;
    }

    private void SelectArts()
    {
        int random = Random.Range(1, csvHolder.CSVDatas.Count - 1);
        if (m_lastArts != random) m_lastArts = random;
        else SelectArts();
    }
}
