using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intimidate_Script : MonoBehaviour
{
    private float timer = 0;
    public float timerMax = 1.0f;

    public bool DeleteOnFinish = true;

    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponentInChildren<Renderer>();

        rend.material.SetFloat("_DissolveProgress", 1);
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
                rend.material.SetFloat("_DissolveProgress", 1);
            }
        }
        else
        {

            if (timerMax * 0.8f <= timer)
            {
                if (timer - Time.deltaTime <= timerMax * 0.8f)
                {

                }
                float InnerProgress = (timer - timerMax * 0.8f) / (timerMax * (1 - 0.8f));

                rend.material.SetFloat("_DissolveProgress", InnerProgress);

            }
            if (timerMax * 0.2f <= timer)
            {
                if (timer - Time.deltaTime <= timerMax * 0.2f)
                {
                    rend.material.SetFloat("_DissolveProgress", 0);
                }
                float InnerProgress = (timer - timerMax * 0.2f) / (timerMax * (0.8f - 0.2f));


            }
            else
            {
                float InnerProgress = (timer) / (timerMax * 0.2f);

                rend.material.SetFloat("_DissolveProgress", 1 - InnerProgress);
            }

        }


    }

}
