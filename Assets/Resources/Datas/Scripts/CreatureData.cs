using UnityEngine;

[SerializeField]
[CreateAssetMenu(fileName = "CreatureData", menuName = "CreatureData")]
public class CreatureData : ScriptableObject
{
    public CreatureDataEntity data;
}
