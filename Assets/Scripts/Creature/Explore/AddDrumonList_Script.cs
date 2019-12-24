using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class AddDrumonList_Script : MonoBehaviour
{
    
    private List<SearchEnemy_Script> m_drumonList = new List<SearchEnemy_Script>();

    public List<SearchEnemy_Script> DrumonList
    {
        get{return m_drumonList;}
    }
}
