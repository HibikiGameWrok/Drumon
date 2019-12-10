using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStart_Script : MonoBehaviour
{
    [SerializeField]
    PlayerPosition_Script m_position = null;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = m_position.Position;
    }
}
