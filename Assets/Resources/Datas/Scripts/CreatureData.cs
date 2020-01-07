﻿using UnityEngine;

public class CreatureData : ScriptableObject
{
    public string name = "";

    public int level = 0;

    public int maxHp = 0;

    public int hp = 0;

    public int atk = 0;

    public int def = 0;

    public float waitTime = 0.0f;

    public CreatureDataEntity.ELEM elem = 0;
}
