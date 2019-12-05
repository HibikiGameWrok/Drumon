/*----------------------------------------------------------*/
//  file:      LookAt_Scripts.cs		                                |
//				 											                    |
//  brief:    特定の方向に向けるスクリプト		                | 
//															                    |
//  date:	2019.12.4									                |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LookAt_Script : MonoBehaviour
{
    [SerializeField]
    private Transform m_head = null;
    [SerializeField]
    private Vector3 m_lookAtTargetPosition;
    [SerializeField]
    private float m_lookAtCoolTime = 0.2f;
    [SerializeField]
    private float m_lookAtHeatTime = 0.2f;
    [SerializeField]
    private bool m_looking = true;

    private Vector3 m_lookAtPositon;
    private Animator m_animator;
    private float m_lookAtWeight = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if((!m_head))
        {
            enabled = false;
            return;
        }

        m_animator = GetComponent<Animator>();

        m_lookAtTargetPosition = m_head.position + transform.forward;
        m_lookAtPositon = m_lookAtTargetPosition;
    }

    void OnAnimatorIK()
    {
        m_lookAtTargetPosition.y = m_head.position.y;
        float lookAtTragetWeight = m_looking ? 1.0f : 0.0f;

        Vector3 curDir = m_lookAtPositon - m_head.position;
        Vector3 futDir = m_lookAtPositon - m_head.position;

        curDir = Vector3.RotateTowards(
            curDir, 
            futDir, 
            6.28f * Time.deltaTime, 
            float.PositiveInfinity
            );
        m_lookAtPositon = m_head.position + curDir;

       // float blendTime = lookAtTragetWeight
    }
}
