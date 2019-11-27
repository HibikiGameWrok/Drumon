using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[CreateAssetMenu(fileName = "", menuName = "PlayerBox")]
public class PlayerBox_Script : ScriptableObject
{
    [SerializeField]
    private CreatureData[] dataList = new CreatureData[6];

    public CreatureData[] DataList
    {
        get { return dataList; }
    }
}
