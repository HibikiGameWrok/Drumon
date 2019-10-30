using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[CreateAssetMenu(fileName = "", menuName = "PlayerBox")]
public class PlayerBox_Script : ScriptableObject
{
    [SerializeField]
    private CharactorData[] dataList = new CharactorData[6];

    public CharactorData[] DataList
    {
        get { return dataList; }
    }
}
