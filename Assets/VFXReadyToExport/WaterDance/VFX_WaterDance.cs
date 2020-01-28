using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INSTRUCTIONS:
//Spawn the provided object above the user Drumon

public class VFX_WaterDance : MonoBehaviour
{
    [SerializeField]
    private Transform CloudContainer = null;
    [SerializeField]
    private Transform TornadoContainer = null;
    [SerializeField]
    private ParticleSystem RainParticles = null;
    [SerializeField]
    private ParticleSystem HealingParticles = null;

    private Renderer[] rend_Clouds = null;
    private Renderer[] rend_Tornadoes = null;

    private float timer = 0;
    public float timerMax = 1.5f;

    public bool DeleteOnFinish = true;

    // Start is called before the first frame update
    void Start()
    {
        rend_Clouds = CloudContainer.GetComponentsInChildren<Renderer>();
        rend_Tornadoes = TornadoContainer.GetComponentsInChildren<Renderer>();

        for (int i = 0; i < rend_Clouds.Length; ++i)
        {
            rend_Clouds[i].material.SetFloat("_DissolveProgress", 1);
            rend_Clouds[i].material.SetFloat("_CloudColorTransition", 0);
        }
        for (int i = 0; i < rend_Tornadoes.Length; ++i)
        {
            rend_Tornadoes[i].transform.localScale = new Vector3(0, 2, 0);
            rend_Tornadoes[i].material.SetFloat("_TestCutoffVar", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer < 0) return;

        if (timer >= timerMax)
        {
            if (DeleteOnFinish)
            {
                Destroy(gameObject);
            }
            else
            {
                timer = 0;

                for (int i = 0; i < rend_Clouds.Length; ++i)
                {
                    rend_Clouds[i].material.SetFloat("_DissolveProgress", 1);
                    rend_Clouds[i].material.SetFloat("_CloudColorTransition", 0);
                }
                for (int i = 0; i < rend_Tornadoes.Length; ++i)
                {
                    rend_Tornadoes[i].material.SetFloat("_TestCutoffVar", 0);
                }
            }
        }
        else
        {
            //Dark Clouds Dissolve In (0.00)
            //Tornadoes Fade In (0.16)
            //Rain Falls (0.33)
            //Heal Appears (0.50)
            //Tornadoes Fade Out and Clouds turn Normal (0.66)
            //Clouds Dissolve Out (0.83)

            if (timer >= timerMax * 0.83f)
            {
                if (timerMax * 0.83f >= timer - Time.deltaTime)
                {
                    for (int i = 0; i < rend_Clouds.Length; ++i)
                    {
                        rend_Clouds[i].material.SetFloat("_CloudColorTransition", 1);
                    }
                    for (int i = 0; i < rend_Tornadoes.Length; ++i)
                    {
                        rend_Tornadoes[i].transform.localScale = new Vector3(0, 2, 0);
                        rend_Tornadoes[i].material.SetFloat("_TestCutoffVar", 0);
                    }
                }
                float InnerProgress = (timer - timerMax * 0.83f) / (timerMax * (1 - 0.83f));

                for (int i = 0; i < rend_Clouds.Length; ++i)
                {
                    rend_Clouds[i].material.SetFloat("_DissolveProgress", InnerProgress);
                }

            }
            else if (timer >= timerMax * 0.66f)
            {
                if (timerMax * 0.66f >= timer - Time.deltaTime)
                {
                    
                }
                float InnerProgress = (timer - timerMax * 0.66f) / (timerMax * (0.83f - 0.66f));

                for (int i = 0; i < rend_Clouds.Length; ++i)
                {
                    rend_Clouds[i].material.SetFloat("_CloudColorTransition", InnerProgress);
                }
                for (int i = 0; i < rend_Tornadoes.Length; ++i)
                {
                    rend_Tornadoes[i].transform.localScale = new Vector3(1 - InnerProgress, 2, 1 - InnerProgress);
                    rend_Tornadoes[i].material.SetFloat("_TestCutoffVar", 1 - InnerProgress);
                }

            }
            else if (timer >= timerMax * 0.5f)
            {
                if (timerMax * 0.5f >= timer - Time.deltaTime)
                {
                    
                }
                float InnerProgress = (timer - timerMax * 0.5f) / (timerMax * (0.66f - 0.5f));



            }
            else if (timer >= timerMax * 0.33f)
            {
                if (timerMax * 0.33f >= timer - Time.deltaTime)
                {
                    HealingParticles.Play();
                    for (int i = 0; i < rend_Tornadoes.Length; ++i)
                    {
                        rend_Tornadoes[i].transform.localScale = new Vector3(1, 2, 1);
                        rend_Tornadoes[i].material.SetFloat("_TestCutoffVar", 1);
                    }
                }
                float InnerProgress = (timer - timerMax * 0.33f) / (timerMax * (0.5f - 0.33f));



            }
            else if (timer >= timerMax * 0.16f)
            {
                if (timerMax * 0.16f >= timer - Time.deltaTime)
                {
                    RainParticles.Play();
                    for (int i = 0; i < rend_Clouds.Length; ++i)
                    {
                        rend_Clouds[i].material.SetFloat("_DissolveProgress", 0);
                    }
                }
                float InnerProgress = (timer - timerMax * 0.16f) / (timerMax * (0.33f - 0.16f));

                for (int i = 0; i < rend_Tornadoes.Length; ++i)
                {
                    rend_Tornadoes[i].transform.localScale = new Vector3(InnerProgress, 2, InnerProgress);
                    rend_Tornadoes[i].material.SetFloat("_TestCutoffVar", InnerProgress);
                }

            }
            else if (timer >= timerMax * 0.0f)
            {
                //if (timerMax * 0.0f >= timer - Time.deltaTime)
                //{

                //}
                float InnerProgress = (timer - timerMax * 0.0f) / (timerMax * (0.16f - 0.0f));

                for (int i = 0; i < rend_Clouds.Length; ++i)
                {
                    rend_Clouds[i].material.SetFloat("_DissolveProgress", 1 - InnerProgress);
                }

            }


        }

    }
}
