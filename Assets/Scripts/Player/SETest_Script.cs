using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SETest_Script : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_donSE = null;

    private AudioSource audioSource = null;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    audioSource.PlayOneShot(m_donSE);
        //}
    }

    // 当たり判定
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "DistanceGrabHandLeft")
        {
            audioSource.PlayOneShot(m_donSE);
        }
        if (collision.gameObject.name == "DistanceGrabHandRight")
        {
            audioSource.PlayOneShot(m_donSE);
        }
    }
}
