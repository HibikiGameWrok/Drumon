using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager_Script : SingletonBase_Script<BattleManager_Script>
{
    private PlayerCreature_Script m_playerCreature;
    private EnemyCreature_Script m_enemyCreature;

    private bool m_isSetting;

    // Start is called before the first frame update
    void Start()
    {
        this.m_playerCreature = null;
        this.m_enemyCreature = null;

        this.m_isSetting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isSetting)
        {
            this.m_playerCreature.Execute();
            this.m_enemyCreature.Execute();
            this.Result();
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

    private void Result()
    {
        if (!this.m_playerCreature) Debug.Log("Lose");
        else if (!this.m_enemyCreature) Debug.Log("Win");
    }
}
