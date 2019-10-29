using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXWindTunnelShaderControl_Script : MonoBehaviour
{
    [Range(0.1f, 10)]
    public float m_maxTime = 0.5f;
    [Range(5, 20)]
    public float m_distance = 8;

    private float Progress = 0.5f;
    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        var the = transform.localScale;
        the.y = m_distance;
        transform.localScale = the;

        rend = transform.GetChild(0).gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Progress += Time.deltaTime * (m_distance / m_maxTime);

        if (Progress >= m_distance) Destroy(gameObject);
        else rend.material.SetFloat("_Progress", Progress);
    }
}
