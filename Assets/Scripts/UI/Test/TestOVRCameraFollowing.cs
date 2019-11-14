using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

#pragma warning disable 618

public class TestOVRCameraFollowing : MonoBehaviour
{
    private GameObject m_localAvatar = null;
    private Transform m_oVRCameraRig = null;
    private Transform m_trackingSpace = null;
    private Transform m_centerEyeAnchor = null;

    [SerializeField]
    private float m_spaceY = 0.0f;
    private float m_distanceY = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (XRDevice.isPresent)
        {
            m_localAvatar = GameObject.Find("LocalAvatarWithGrab");
            m_oVRCameraRig = m_localAvatar.transform.FindChild("OVRCameraRig");
            m_trackingSpace = m_oVRCameraRig.transform.FindChild("TrackingSpace");
            m_centerEyeAnchor = m_trackingSpace.transform.FindChild("CenterEyeAnchor");

            m_distanceY = Mathf.Abs(m_centerEyeAnchor.transform.position.y) * this.transform.position.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Oculus Riftが接続されているならば
        if (XRDevice.isPresent)
        {
            this.transform.position = new Vector3(this.transform.position.x,m_centerEyeAnchor.transform.position.y - m_distanceY, this.transform.position.z);
        }
    }
}
