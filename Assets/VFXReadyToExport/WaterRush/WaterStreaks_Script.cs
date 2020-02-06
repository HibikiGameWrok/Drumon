using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterStreaks_Script : MonoBehaviour
{

    public float timer = 0.0f;
    public float timerMax = 0.4f;

    private Renderer theRenderer = null;

    public float speed = 64;

    [SerializeField]
    private GameObject WaterImpact_Prefab = null;

    // Start is called before the first frame update
    void Start()
    {
        theRenderer = GetComponentInChildren<Renderer>();

        theRenderer.material.SetFloat("_CutoffFromBottom", 1);
        theRenderer.material.SetFloat("_TestVariable", 0);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timerMax)
        {
            Destroy(gameObject);
        }
        else
        {
            if (timer >= timerMax * 0.65f)
            {
                if (timerMax * 0.65f >= timer - Time.deltaTime)
                {
                    GameObject waterimpact = Instantiate(WaterImpact_Prefab);
                    waterimpact.transform.position = transform.position;
                    waterimpact.transform.rotation = transform.rotation;
                    waterimpact.transform.Rotate(-90, 0, 0);
                    waterimpact.transform.position += transform.up * transform.localScale.y;
                }
                float InnerProgress = (timer - timerMax * 0.65f) / (timerMax * (1 - 0.65f));

                theRenderer.material.SetFloat("_TestVariable", InnerProgress);

            }
            else if (timer >= timerMax * 0.1f)
            {
                if (timerMax * 0.1f >= timer - Time.deltaTime)
                {
                    theRenderer.material.SetFloat("_CutoffFromBottom", 0);
                }
                float InnerProgress = (timer - timerMax * 0.1f) / (timerMax * (0.65f - 0.1f));



            }
            else
            {
                float InnerProgress = (timer) / (timerMax * (0.1f));

                theRenderer.material.SetFloat("_CutoffFromBottom", 1 - InnerProgress);

            }

        }

        transform.position += transform.up * speed * Time.deltaTime;
    }

}
