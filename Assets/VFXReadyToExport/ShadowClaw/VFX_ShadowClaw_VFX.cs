using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_ShadowClaw_VFX : MonoBehaviour
{
    [SerializeField]
    private Transform ClawHolder0 = null;

    private Renderer[] rend_clawholder0 = null;

    private float timer = 0;
    public float timerMax = 0.5f;

    public bool DeleteOnFinish = true;

    [SerializeField]
    private GameObject Impact_Prefab = null;

    [SerializeField]
    private GameObject ParticleHolder0 = null, ParticleHolder1 = null, ParticleHolder2 = null;

    // Start is called before the first frame update
    void Start()
    {
        rend_clawholder0 = ClawHolder0.GetComponentsInChildren<Renderer>();

        for (int i = 0; i < rend_clawholder0.Length; ++i)
        {
            rend_clawholder0[i].material.SetFloat("_Progress", 0.5f);
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

            float CalculatedProgress = Mathf.Clamp(1.5f * timer / timerMax, 0, 1);
            for (int i = 0; i < rend_clawholder0.Length; ++i)
            {
                rend_clawholder0[i].material.SetFloat("_Progress", 0.5f + CalculatedProgress);
            }
            if (CalculatedProgress == 0 || CalculatedProgress == 1)
            {
                if (ParticleHolder0.GetComponent<ParticleSystem>().isPlaying)
                {
                    ParticleHolder0.SetActive(false);
                    ParticleHolder1.SetActive(false);
                    ParticleHolder2.SetActive(false);
                    //ParticleHolder0.GetComponent<ParticleSystem>().Stop();
                    //ParticleHolder1.GetComponent<ParticleSystem>().Stop();
                    //ParticleHolder2.GetComponent<ParticleSystem>().Stop();
                }
            }
            else
            {
                if (!ParticleHolder0.GetComponent<ParticleSystem>().isPlaying)
                {
                    ParticleHolder0.SetActive(true);
                    ParticleHolder1.SetActive(true);
                    ParticleHolder2.SetActive(true);
                    //ParticleHolder0.GetComponent<ParticleSystem>().Play();
                    //ParticleHolder1.GetComponent<ParticleSystem>().Play();
                    //ParticleHolder2.GetComponent<ParticleSystem>().Play();
                }

                float distancevalue = -2.2f * CalculatedProgress;
                var thepos = ParticleHolder0.transform.localPosition;
                thepos.y = distancevalue;
                ParticleHolder0.transform.localPosition = thepos;
                thepos = ParticleHolder1.transform.localPosition;
                thepos.y = distancevalue;
                ParticleHolder1.transform.localPosition = thepos;
                thepos = ParticleHolder2.transform.localPosition;
                thepos.y = distancevalue;
                ParticleHolder2.transform.localPosition = thepos;

            }
        }

    }
}
