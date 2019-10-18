using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager_Script : SingletonBase_Script<BattleManager_Script>
{
    private PlayerCreature_Script m_playerCreature;
    private EnemyCreature_Script m_enemyCreature;

    // Start is called before the first frame update
    void Start()
    {
        this.m_playerCreature = null;
        this.m_enemyCreature = null;
    }

    // Update is called once per frame
    void Update()
    {
        this.m_playerCreature.Execute();
        this.m_enemyCreature.Execute();
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
    }
}
