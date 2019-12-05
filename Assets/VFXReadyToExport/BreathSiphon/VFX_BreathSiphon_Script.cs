using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_BreathSiphon_Script : MonoBehaviour
{

    private float timer = 0.0f;
    private float timerMax = 2.0f;

    [SerializeField]
    private GameObject obj_streakholder;
    [SerializeField]
    private Renderer rend_streaks;
    [SerializeField]
    private GameObject obj_AirSphere;
    [SerializeField]
    private Renderer rend_AirSphere;
    [SerializeField]
    private ParticleSystem ps_BreathSiphon;

    [SerializeField]
    private Transform TargetDrumon;

    private Vector3 Origin;
    public float SphereHeight = 12;

    public bool DeleteOnFinish = true;

    public void SetTargetDrumon(Transform target)
    {
        TargetDrumon = target;
        Origin = TargetDrumon.position;
    }

    void InitVFX()
    {
        obj_AirSphere.transform.localScale = new Vector3(8, 8, 8);
        rend_streaks.material.SetFloat("_Brightness", 0);
        rend_AirSphere.material.SetFloat("_Brightness", 0);
        rend_AirSphere.material.SetFloat("_DissolveProgress", 0);
        obj_AirSphere.transform.localPosition = new Vector3(0, 0, 0);
        TargetDrumon.transform.position = obj_AirSphere.transform.position;
        obj_streakholder.transform.localScale = new Vector3(1, 1, 1);
        ps_BreathSiphon.Stop();
    }

    // Start is called before the first frame update
    void Start()
    {
        Origin = TargetDrumon.position;
        InitVFX();
    }

    // Update is called once per frame
    void Update()
    {
        //1. Streaks appear, Sphere appears, Sphere rises up with object
        //2. Start Particle System, and pause at the right time
        //3. Continue the Particle System
        //4. Sphere falls quickly to the ground and dissolves out/expands, streaks increase height then decrease height and goes back down

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
                timer = -0.2f;
                InitVFX();
            }
        }
        else
        {
            if (timer >= timerMax * 0.80f)
            {
                if (timerMax * 0.80f >= timer - Time.deltaTime)
                {
                    obj_AirSphere.transform.localPosition = new Vector3(0, 0, 0);
                    TargetDrumon.transform.position = obj_AirSphere.transform.position;
                }
                float InnerProgress = (timer - timerMax * 0.80f) / (timerMax * (1.00f - 0.80f));

                obj_AirSphere.transform.localScale = new Vector3(8, 8, 8) * (InnerProgress * 8);
                rend_AirSphere.material.SetFloat("_DissolveProgress", InnerProgress * 2);
                rend_AirSphere.material.SetFloat("_Brightness", 1 - 2 * InnerProgress);

                float streakyscalevalue; //Intention: 1 -> 5 -> 0
                
                if (InnerProgress >= 0.5f)
                {
                    streakyscalevalue = 5 - 5 * ((InnerProgress - 0.5f) / 0.5f);
                }
                else
                {
                    streakyscalevalue = 1 + 4 * (InnerProgress / 0.5f);
                }

                obj_streakholder.transform.localScale = new Vector3(1, streakyscalevalue, 1);

            }
            else if (timer >= timerMax * 0.75f)
            {
                if (timerMax * 0.75f >= timer - Time.deltaTime)
                {
                    
                }
                float InnerProgress = (timer - timerMax * 0.75f) / (timerMax * (0.80f - 0.75f));
                
                obj_AirSphere.transform.localPosition = new Vector3(0, (1 - InnerProgress) * SphereHeight, 0);
                TargetDrumon.transform.position = obj_AirSphere.transform.position;

            }
            else if (timer >= timerMax * 0.25f)
            {
                if (timerMax * 0.25f >= timer - Time.deltaTime)
                {
                    rend_streaks.material.SetFloat("_Brightness", 1.5f);
                    rend_AirSphere.material.SetFloat("_Brightness", 1);
                    obj_AirSphere.transform.localPosition = new Vector3(0, SphereHeight, 0);
                    TargetDrumon.transform.position = obj_AirSphere.transform.position;
                }
                float InnerProgress = (timer - timerMax * 0.75f) / (timerMax * (0.75f - 0.25f));


                if (timer >= timerMax * 0.65f && timerMax * 0.65f >= timer - Time.deltaTime)
                {
                    ps_BreathSiphon.Play();
                }
                else if (timer >= timerMax * 0.35f && timerMax * 0.35f >= timer - Time.deltaTime)
                {
                    ps_BreathSiphon.Pause();
                }


            }
            else
            {
                if (0.0f >= timer - Time.deltaTime)
                {

                }
                float InnerProgress = (timer) / (timerMax * (0.25f - 0.00f));

                rend_streaks.material.SetFloat("_Brightness", InnerProgress * 1.5f);
                rend_AirSphere.material.SetFloat("_Brightness", InnerProgress);
                obj_AirSphere.transform.localPosition = new Vector3(0, InnerProgress * SphereHeight, 0);
                TargetDrumon.transform.position = obj_AirSphere.transform.position;

                if (timer >= timerMax * 0.15f && timerMax * 0.15f >= timer - Time.deltaTime)
                {
                    ps_BreathSiphon.Play();
                }

            }

        }


    }
}
