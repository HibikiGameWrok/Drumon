using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TestOVRGetPosition : MonoBehaviour
{
    //　自身の親のオブジェクト情報を保持
    private GameObject m_parentObject;

    // Start is called before the first frame update
    void Start()
    {
        // 親オブジェクトを取得
        m_parentObject = this.transform.parent.gameObject;
        foreach(Transform T in m_parentObject.transform.parent)
        {
            m_parentObject.transform.parent = T.transform.parent;
            Debug.Log("Parent : " + T.parent.name);
        }

        Debug.Log("Parent : " + m_parentObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        // Oculus Riftが接続されているならば
        if (XRDevice.isPresent)
        {
            Debug.Log("CenterPos : " + m_parentObject.transform.TransformPoint(this.transform.position));
        }
    }
}
