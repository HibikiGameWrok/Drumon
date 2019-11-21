using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos_Script : MonoBehaviour
{
    [SerializeField]
    private PlayerPosition_Script m_pos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_pos.Position = this.transform.position;
    }
}
