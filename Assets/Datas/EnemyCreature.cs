using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[CreateAssetMenu(fileName = "", menuName = "EnemyCreature")]
public class EnemyCreature : ScriptableObject
{
    [SerializeField]
    private GameObject enemyCreatureObj;

    public GameObject EnemyCreatureObj
    {
        get { return enemyCreatureObj; }
    }
}
