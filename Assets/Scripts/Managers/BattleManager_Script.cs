using UnityEngine;
using UniRx;

public class BattleManager_Script : SingletonBase_Script<BattleManager_Script>
{
    [SerializeField]
    private PlayerCreature_Script m_playerCreature = null;

    public PlayerCreature_Script PlayerCreature
    {
        get { return m_playerCreature; }
    }

    [SerializeField]
    private EnemyCreature_Script m_enemyCreature = null;

    public EnemyCreature_Script EnemyCreature
    {
        get { return m_enemyCreature; }
    }

    private ICreature_Script m_nowMove = null;
    private ICreature_Script m_nextMove = null;

    private bool m_isSetting;

    private float m_attackSpan;

    private int m_enemyLevel;

    [SerializeField]
    private BoolReactiveProperty m_isFinish = new BoolReactiveProperty(false);

    public IReadOnlyReactiveProperty<bool> IsFinish => m_isFinish;

    // Start is called before the first frame update
    void Start()
    {
        this.m_nowMove = null;
        this.m_nextMove = null;

        this.m_isSetting = false;

        this.m_attackSpan = 0.0f;
        this.m_enemyLevel = m_enemyCreature.GetData().level;
        this.SetTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isSetting)
        {
            this.m_attackSpan -= Time.deltaTime;

            if (this.JudgeResult()) return;
            this.m_playerCreature.Execute();
            this.m_enemyCreature.Execute();
            if (this.m_playerCreature.AtkFlag) SetActive(this.m_playerCreature);
            if (this.m_enemyCreature.AtkFlag) SetActive(this.m_enemyCreature);
            if (this.m_nowMove != null && this.m_attackSpan <= 0.0f) this.Action();
        }
    }

    public void SetPlayerCreature(PlayerCreature_Script creature)
    {
        this.m_playerCreature = creature;
        if (this.m_enemyCreature) this.SetTarget();
    }

    public void SetEnemyCreature(EnemyCreature_Script creature)
    {
        this.m_enemyCreature = creature;
        if (this.m_playerCreature) this.SetTarget();
    }

    private void SetTarget()
    {
        this.m_playerCreature.SetTarget(this.m_enemyCreature);
        this.m_enemyCreature.SetTarget(this.m_playerCreature);

        this.m_isSetting = true;
    }

    public void SetActive(ICreature_Script creature)
    {
        if (this.m_nowMove == creature || this.m_nextMove == creature) return;

        if(this.m_nowMove == null) this.m_nowMove = creature;
        else this.m_nextMove = creature;
    }

    private void Action()
    {
        this.m_nowMove.Attack();
        if (m_nextMove != null)
        {
            this.m_nowMove = this.m_nextMove;
            this.m_nextMove = null;
        }
        else
        {
            this.m_nowMove = null;
        }

        this.m_attackSpan = 1.0f;
    }

    private bool JudgeResult()
    {
        if (this.m_isSetting)
        {
            if (!this.m_playerCreature)
            {
                this.m_isSetting = false;
                m_isFinish.SetValueAndForceNotify(true);
                return true;
            }
            else if (!this.m_enemyCreature)
            {
                this.m_isSetting = false;
                this.m_playerCreature.AddExpPoint();
                m_isFinish.SetValueAndForceNotify(true);
                return true;
            }
        }

        return false;
    }
}
