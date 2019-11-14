using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

#pragma warning disable 618

public class DebugOVRCheckPos_Script : MonoBehaviour
{
    private const string NEWLINESYMBOL = "\n";

    [SerializeField]
    private const string ONELINETITLE = "InitializePos";
    [SerializeField]
    private const string TOWLINETITLE = "UpdatePos";

    private Text m_Debugtext;

    private GameObject m_drummaneger = null;

    private GameObject m_localAvatar = null;
    private Transform m_oVRCameraRig = null;
    private Transform m_trackingSpace = null;
    private Transform m_centerEyeAnchor = null;

    // Start is called before the first frame update
    void Start()
    {
        m_Debugtext = this.GetComponent<Text>();
        m_Debugtext.text = "";

        m_drummaneger = GameObject.Find("DrumManager");

        m_localAvatar = GameObject.Find("LocalAvatarWithGrab");
        m_oVRCameraRig = m_localAvatar.transform.FindChild("OVRCameraRig");
        m_trackingSpace = m_oVRCameraRig.transform.FindChild("TrackingSpace");
        m_centerEyeAnchor = m_trackingSpace.transform.FindChild("CenterEyeAnchor");


        SetLineText(ONELINETITLE, true);
        SetLineText("NotLocalPos");
        Vector3 notLocalPos = new Vector3(this.transform.position.x, m_centerEyeAnchor.transform.position.y - (Mathf.Abs(m_centerEyeAnchor.transform.position.y) * m_drummaneger.transform.position.y), this.transform.position.z);
        SetLineText(notLocalPos.ToString(), true);
        SetLineText("LocalPos");
        Vector3 lLocalPos = new Vector3(this.transform.position.x, InputTracking.GetLocalPosition(XRNode.CenterEye).y - (Mathf.Abs(InputTracking.GetLocalPosition(XRNode.CenterEye).y) * m_drummaneger.transform.position.y), this.transform.position.z);
        SetLineText(lLocalPos.ToString());
        SetLineText("  ", true);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 notLocalPos = new Vector3(this.transform.position.x, m_centerEyeAnchor.transform.position.y - (Mathf.Abs(m_centerEyeAnchor.transform.position.y) * m_drummaneger.transform.position.y), this.transform.position.z);
        Vector3 lLocalPos = new Vector3(this.transform.position.x, InputTracking.GetLocalPosition(XRNode.CenterEye).y - (Mathf.Abs(InputTracking.GetLocalPosition(XRNode.CenterEye).y) * m_drummaneger.transform.position.y), this.transform.position.z);
        m_Debugtext.text = notLocalPos.ToString() + ":" + lLocalPos.ToString();
    }
        
    /// <summary>
    /// Textコンポーネントに文を追加する関数
    /// </summary>
    /// <param name="text">文章</param>
    /// <param name="newlineFlag">true = 改行,false = 改行なし,(デフォルト引数 = false)</param>
    void SetLineText(string text, bool newlineFlag = false)
    {
        m_Debugtext.text = m_Debugtext.text + text;
        if(newlineFlag == true)
        {
            m_Debugtext.text = m_Debugtext.text + NEWLINESYMBOL;
        }
    }

}
