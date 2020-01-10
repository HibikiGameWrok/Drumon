using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffRoll_TextMove_Script : MonoBehaviour
{
    [SerializeField]
    private float m_moveSpeed = 0.0f;

    private Vector3 m_movePos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        m_movePos = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.localPosition.y < 400)
        {
            m_movePos.y += Time.deltaTime * m_moveSpeed;
            this.transform.localPosition = new Vector3(0.0f, m_movePos.y, 0.0f);
        }
        else if(this.transform.localPosition.y >= 400)
        {
            Destroy(this.gameObject);
        }
        Debug.Log(this.transform.localPosition.y);
    }
}
