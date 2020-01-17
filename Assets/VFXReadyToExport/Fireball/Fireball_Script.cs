using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball_Script : MonoBehaviour
{
    private float timer = 0.0f;
    public float timerMax = 0.4f;

    public float m_speed = 10;

    public bool DeleteOnFinish = true;

    private Vector3 Origin = new Vector3();

    [SerializeField]
    private GameObject Trail_Prefab = null;
    public int AmountOfTrails = 3;

    private TrailRenderer[] trails = null;

    [SerializeField]
    private GameObject Impact_Prefab = null;

    [SerializeField]
    private GameObject SphereHolder = null;
    [SerializeField]
    private Renderer rend_Fire = null;

    // Start is called before the first frame update
    void Start()
    {
        Origin = transform.position;

        for (int i = 0; i < AmountOfTrails; ++i)
        {
            GameObject trailobj = Instantiate(Trail_Prefab, transform);
            trailobj.transform.localPosition = new Vector3(Random.Range(-0.5f, 0.5f), transform.localScale.y, Random.Range(-0.5f, 0.5f));
        }
        trails = GetComponentsInChildren<TrailRenderer>();

        SphereHolder.transform.localScale = new Vector3(0, 0, 0);
        rend_Fire.material.SetFloat("_DissolveProgress", 1);
        
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

                transform.position = Origin;

                for (int i = 0; i < AmountOfTrails; ++i)
                {
                    GameObject trailobj = Instantiate(Trail_Prefab, transform);
                    trailobj.transform.localPosition = new Vector3(Random.Range(-0.7f, 0.7f), transform.localScale.y, Random.Range(-0.7f, 0.7f));
                }
                trails = GetComponentsInChildren<TrailRenderer>();

                SphereHolder.transform.localScale = new Vector3(0, 0, 0);
                rend_Fire.material.SetFloat("_DissolveProgress", 1);
            }
        }
        else
        {

            if (timerMax * 0.8f <= timer)
            {
                if (timer - Time.deltaTime <= timerMax * 0.8f)
                {
                    if (Impact_Prefab != null)
                    {
                        GameObject impactobj = Instantiate(Impact_Prefab);
                        impactobj.transform.position = transform.position;
                        Destroy(impactobj, 0.3f);
                    }

                    for (int i = 0; i < trails.Length; ++i)
                    {
                        trails[i].transform.SetParent(null);
                        trails[i].GetComponent<TrailRenderer>().emitting = false;
                        Destroy(trails[i].gameObject, 1);
                    }
                }
                float InnerProgress = (timer - timerMax * 0.8f) / (timerMax * (1 - 0.8f));

                SphereHolder.transform.localScale = new Vector3(1, 1, 1) * (1 - InnerProgress);
                rend_Fire.material.SetFloat("_DissolveProgress", 2 * InnerProgress);

            }
            else if (timerMax * 0.2f <= timer)
            {
                if (timer - Time.deltaTime <= timerMax * 0.2f)
                {
                    SphereHolder.transform.localScale = new Vector3(1, 1, 1);
                    rend_Fire.material.SetFloat("_DissolveProgress", 0);
                }

            }
            else
            {
                float InnerProgress = (timer) / (timerMax * 0.2f);

                SphereHolder.transform.localScale = new Vector3(1, 1, 1) * InnerProgress;
                rend_Fire.material.SetFloat("_DissolveProgress", 1 - InnerProgress);
            }


            transform.position += -transform.up * Time.deltaTime * m_speed;
        }
    }
}
