using UnityEngine;

[SerializeField]
[CreateAssetMenu(fileName = "", menuName = "EnemyCreature")]
public class EnemyCreature : ScriptableObject
{
    [SerializeField]
    private CreatureData enemyCreatureData = null;

    public CreatureData EnemyCreatureData
    {
        get { return enemyCreatureData; }
        set { enemyCreatureData = value; }
    }
}
