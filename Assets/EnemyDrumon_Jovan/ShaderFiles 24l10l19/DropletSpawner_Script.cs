using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropletSpawner_Script : MonoBehaviour
{
    private float m_timer;
    public float TimerMax = 0.5f;

    [SerializeField]
    private GameObject m_dropletPrefab;

    private GameObject m_dropletReference;

    // Start is called before the first frame update
    void Start()
    {
        m_dropletReference = Instantiate(m_dropletPrefab, transform);
        m_dropletReference.transform.position = transform.position;

        m_timer = TimerMax + Random.Range(-0.5f, 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        m_timer -= Time.deltaTime;
        if (m_timer <= 0.0)
        {
            m_timer = TimerMax + Random.Range(-0.5f, 2.5f);
            m_dropletReference.transform.position = transform.position;
        }
    }
}
