using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFadeScript : MonoBehaviour
{
    private GameObject m_TK = null;
    private GameObject m_localAvatar = null;
    //[SerializeField]
    //private GameObject m_fadeobject;
    private PanelUI_Fade_Script m_pnaelFadeScript = null;

    // Start is called before the first frame update
    void Start()
    {
        m_TK = GameObject.Find("VRTK");
        m_localAvatar = GameObject.FindGameObjectWithTag("Player");
        m_pnaelFadeScript = this.GetComponent<PanelUI_Fade_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_localAvatar.transform.gameObject.activeSelf == true)
        {
            m_pnaelFadeScript.IsFadeIn = true;
        }
    }
}
