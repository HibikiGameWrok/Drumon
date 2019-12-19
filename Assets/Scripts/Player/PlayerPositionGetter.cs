using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionGetter : SingletonBase_Script<PlayerPositionGetter>
{
    [SerializeField]
<<<<<<< HEAD
    private PlayerPosition_Script m_pos = null;
=======
    private PlayerPosition_Script m_pos;
>>>>>>> feature/audio
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = m_pos.Position;
    }
}
