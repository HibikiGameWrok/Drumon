using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionReceiver_Script : MonoBehaviour
{
    private float m_timeToDisappear = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_timeToDisappear > 0)
        {
            m_timeToDisappear -= Time.deltaTime;
            if (m_timeToDisappear <= 0.0)
            {
                GetComponent<Text>().text = "";
            }
        }
    }

    public void DisplayText(string _text)
    {
        GetComponent<Text>().text = _text;
        m_timeToDisappear = 1.0f;
    }

}
