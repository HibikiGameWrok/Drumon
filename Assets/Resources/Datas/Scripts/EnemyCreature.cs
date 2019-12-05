using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[CreateAssetMenu(fileName = "", menuName = "EnemyCreature")]
public class EnemyCreature : ScriptableObject
{
    [SerializeField]
    private CreatureData enemyCreatureData;

    public CreatureData EnemyCreatureData
    {
        get { return enemyCreatureData; }
        set { enemyCreatureData = value; }
    }
}
