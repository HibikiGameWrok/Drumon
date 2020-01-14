using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButton_Script : MonoBehaviour
{
    [SerializeField]
    private bool m_isStart = false;
    [SerializeField]
    private bool m_isTutorial = false;

    public bool IsStart
    {
        get { return m_isStart; }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClilckedStart()
    {
        m_isStart = true;
    }
}
