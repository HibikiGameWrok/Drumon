using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INSTRUCTIONS:
//Place the provided object at the mouth of the user Drumon
//Make this object's "transform.up" face the target Drumon
//Adjust the Y value of the provided object's Scale to increase the distance

public class VFX_WaterGun_Script : MonoBehaviour
{
    private float timer = 0.0f;
    public float timerMax = 1.0f;

    public bool DeleteOnFinish = true;

    private float InitialYScale = 10;

    [SerializeField]
    private Renderer rend_WaterGun;
    [SerializeField]
    private GameObject WaterImpact_Prefab;

    //Only adjust this if you are not using a scale of 10 for water gun
    public float ImpactPlacementMagnifier = 1.7f;

    // Start is called before the first frame update
    void Start()
    {
        InitialYScale = transform.localScale.y;
        var scale = transform.localScale;
        scale.x = 0;
        scale.z = 0;
        transform.localScale = scale;
        rend_WaterGun.material.SetFloat("_StartCompress", 0);
        rend_WaterGun.material.SetFloat("_EndCompress", 0);
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
                rend_WaterGun.material.SetFloat("_StartCompress", 0);
                rend_WaterGun.material.SetFloat("_EndCompress", 0);

                var scale = transform.localScale;
                scale.x = 0;
                scale.z = 0;
                transform.localScale = scale;
            }
        }
        else
        {
            if (timer >= timerMax * 0.9f)
            {
                if (timerMax * 0.9f >= timer - Time.deltaTime)
                {

                }
                float InnerProgress = (timer - timerMax * 0.9f) / (timerMax * (1 - 0.9f));

                rend_WaterGun.material.SetFloat("_EndCompress", InnerProgress);

            }
            else if (timer >= timerMax * 0.1f)
            {
                if (timerMax * 0.1f >= timer - Time.deltaTime)
                {
                    GameObject waterimpact = Instantiate(WaterImpact_Prefab);
                    waterimpact.transform.position = transform.position + ImpactPlacementMagnifier * transform.up * InitialYScale;
                    waterimpact.transform.rotation = transform.rotation;
                    waterimpact.transform.Rotate(-90, 0, 0);
                    //Destroy(waterimpact, timerMax * 0.9f);

                    rend_WaterGun.material.SetFloat("_StartCompress", 1);

                    var scale = transform.localScale;
                    scale.x = 1;
                    scale.z = 1;
                    transform.localScale = scale;
                }
                float InnerProgress = (timer - timerMax * 0.1f) / (timerMax * (0.9f - 0.1f));

            }
            else
            {
                float InnerProgress = (timer) / (timerMax * 0.1f);

                rend_WaterGun.material.SetFloat("_StartCompress", InnerProgress);

                var scale = transform.localScale;
                scale.x = Mathf.Clamp(InnerProgress * 2, 0, 1);
                scale.z = Mathf.Clamp(InnerProgress * 2, 0, 1);
                transform.localScale = scale;
            }


        }
    }
}
