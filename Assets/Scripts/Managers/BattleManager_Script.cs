using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager_Script : SingletonBase_Script<BattleManager_Script>
{
    private PlayerCreature_Script m_playerCreature;
    private EnemyCreature_Script m_enemyCreature;

    private ICreature_Script m_nowMove;
    private ICreature_Script m_nextMove;

    private bool m_isSetting;

    // Start is called before the first frame update
    void Start()
    {
        this.m_playerCreature = null;
        this.m_enemyCreature = null;

        this.m_nowMove = null;
        this.m_nextMove = null;

        this.m_isSetting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isSetting)
        {
            this.m_playerCreature.Execute();
            this.m_enemyCreature.Execute();
            if (this.m_nowMove != null) this.Action();
        }
    }

    public void SetPlayerCreature(PlayerCreature_Script creature)
    {
        this.m_playerCreature = creature;
        //AttackRecipeManeger_Script.Get.SetCreatureNameSearch(this.m_playerCreature);
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
    }
}
