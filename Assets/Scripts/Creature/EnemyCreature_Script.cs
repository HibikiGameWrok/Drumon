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
        this.m_timer = 0.0f;

        this.m_atkFlag = false;

        m_healProsperityUI = GameObject.Find("ESlider");
        m_healProsperityUIScript = m_healProsperityUI.GetComponent<HealProsperityUI_Script>();
    }

    public void Execute()
    {
        m_healProsperityUIScript.NowPoint = m_data.Hp;
        this.CountTimer();
        if (this.m_timer >= 5.0f) this.m_atkFlag = true;
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
    }

    public void Damage(int damage)
    {
        this.m_data.Hp -= damage;
        GetComponent<ParticleSystem>().Play();
        if (this.m_data.Hp < 0) this.m_data.Hp = 0;
        this.Dead();
    }

    public void Heal()
    {
        this.m_data.Hp += 10;
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
        if (this.m_data.Hp <= 0) Destroy(this.gameObject);
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
        obj = Instantiate(obj, this.transform.position, this.transform.rotation);
        obj.transform.parent = this.gameObject.transform;
    }
}
