using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCursor_Script : MonoBehaviour
{
    [SerializeField]
    private Text[] m_textList = null;

    private int m_selectNum;

    // Start is called before the first frame update
    void Start()
    {
        this.m_selectNum = 0;

        if(this.m_textList[this.m_selectNum])
        {
            this.transform.position = (this.m_textList[this.m_selectNum].transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            this.m_selectNum++;
            if (this.m_selectNum >= m_textList.Length) m_selectNum = 0;
            this.transform.position = (this.m_textList[this.m_selectNum].transform.position);
        }
    }
}
