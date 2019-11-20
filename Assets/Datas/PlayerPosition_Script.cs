using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[CreateAssetMenu(fileName = "", menuName = "PlayerPos")]
public class PlayerPosition_Script : ScriptableObject
{

    private Vector3 position = new Vector3(0, 0, 0);

    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }

    private void OnEnable()
    {
        Position = position;
    }
}
