using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXImpactShaderControl_Script : MonoBehaviour
{
    [Range(0.1f, 2)]
    public float m_maxTime = 0.5f;

    private float Progress = 0.5f;
    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = transform.GetChild(0).gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Progress += 0.5f * Time.deltaTime * (1 / m_maxTime);

        if (Progress >= 1) Destroy(gameObject);
        else rend.material.SetFloat("_Progress", Progress);

    }
}
