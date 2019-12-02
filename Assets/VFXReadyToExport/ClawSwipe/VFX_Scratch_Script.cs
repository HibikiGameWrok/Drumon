using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Scratch_Script : MonoBehaviour
{
    [SerializeField]
    private Transform ClawHolder0, ClawHolder1;

    private Renderer[] rend_clawholder0, rend_clawholder1;

    private float timer = 0;
    public float timerMax = 0.5f;

    public bool DeleteOnFinish = true;

    [SerializeField]
    private GameObject Impact_Prefab;

    // Start is called before the first frame update
    void Start()
    {
        rend_clawholder0 = ClawHolder0.GetComponentsInChildren<Renderer>();
        rend_clawholder1 = ClawHolder1.GetComponentsInChildren<Renderer>();
        
        for (int i = 0; i < rend_clawholder0.Length; ++i)
        {
            rend_clawholder0[i].material.SetFloat("_Progress", 0.5f);
        }
        for (int i = 0; i < rend_clawholder1.Length; ++i)
        {
            rend_clawholder1[i].material.SetFloat("_Progress", 0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timerMax)
        {
            if (DeleteOnFinish)
            {
                Destroy(gameObject);
            }
            else
            {
                timer = 0;
                for (int i = 0; i < rend_clawholder0.Length; ++i)
                {
                    rend_clawholder0[i].material.SetFloat("_Progress", 0.5f);
                }
                for (int i = 0; i < rend_clawholder1.Length; ++i)
                {
                    rend_clawholder1[i].material.SetFloat("_Progress", 0.5f);
                }
            }

        }
        else
        {
            if (timer - Time.deltaTime <= timerMax * 0.2f && timer >= timerMax * 0.2f)
            {
                GameObject impactobj = Instantiate(Impact_Prefab);
                impactobj.transform.position = transform.position + transform.forward * 2.5f + new Vector3(Random.Range(-0.8f, 0.8f), Random.Range(-0.8f, 0.8f), Random.Range(-0.8f, 0.8f));
                impactobj.transform.rotation = transform.rotation;
                impactobj.transform.localScale *= 0.4f;
                Destroy(impactobj, 0.3f);
            }
            if (timer - Time.deltaTime <= timerMax * 0.4f && timer >= timerMax * 0.4f)
            {
                GameObject impactobj = Instantiate(Impact_Prefab);
                impactobj.transform.position = transform.position + transform.forward * 2.5f + new Vector3(Random.Range(-0.8f, 0.8f), Random.Range(-0.8f, 0.8f), Random.Range(-0.8f, 0.8f));
                impactobj.transform.rotation = transform.rotation;
                impactobj.transform.localScale *= 0.4f;
                Destroy(impactobj, 0.3f);
            }

            for (int i = 0; i < rend_clawholder0.Length; ++i)
            {
                rend_clawholder0[i].material.SetFloat("_Progress", 0.5f + Mathf.Clamp(1.5f * timer / timerMax, 0, 1));
            }
            for (int i = 0; i < rend_clawholder1.Length; ++i)
            {
                rend_clawholder1[i].material.SetFloat("_Progress", 0.5f + Mathf.Clamp(1.5f * timer / timerMax - 0.3f, 0, 1));
            }
        }

    }
}
