using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
[CreateAssetMenu(fileName = "CharactorData",menuName = "CharactorData")]
public class CharactorData : ScriptableObject
{
    public enum ELEM
    {
        FIRE,
        WIND,
        VOID,
        EARTH,
        WATER
    };

    [SerializeField]
    private int hp = 0;

    public int Hp
    {
        get { return hp; }
    }

    [SerializeField]
    private int atk = 0;

    public int Atk
    {
        get { return atk; }
    }

    [SerializeField]
    private int def = 0;

    public int Def
    {
        get { return def; }
    }

    //[SerializeField]
    //private float wait;

    //public float Wait
    //{
    //    get { return wait; }
    //    set { wait = value; }
    //}

    [SerializeField]
    private ELEM elem = 0;

    public ELEM Elem
    {
        get { return elem; }
    }
}
