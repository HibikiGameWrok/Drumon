using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INSTRUCTIONS:
//Spawn the provided object at the user Drumon
//Make this object face the target

public class VFX_DarkSphere_Script : MonoBehaviour
{
    private float timer = 0.0f;
    public float timerMax = 1.5f;

    public bool DeleteOnFinish = true;

    private Vector3 Origin = new Vector3();
    public float m_speed = 1;
    private Vector3 OriginalScale = new Vector3();
    
    [SerializeField]
    private GameObject Impact_Prefab = null;

    [SerializeField]
    private Renderer rend_sphere0 = null;
    [SerializeField]
    private Renderer rend_sphere1 = null;
    [SerializeField]
    private Renderer rend_torus0 = null;
    [SerializeField]
    private Renderer rend_torus1 = null;
    [SerializeField]
    private Renderer rend_torus2 = null;
    [SerializeField]
    private ParticleSystem ChargeUp_Particle = null;

    // Start is called before the first frame update
    void Start()
    {
        Origin = transform.localPosition;
        OriginalScale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);
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
                transform.localScale = new Vector3(0, 0, 0);
                transform.localPosition = Origin;

                rend_sphere0.material.SetFloat("_DissolveProgress", 0);
                rend_sphere1.material.SetFloat("_DissolveProgress", 0);

                rend_torus0.material.SetFloat("_Brightness", 3);
                rend_torus1.material.SetFloat("_Brightness", 3);
                rend_torus2.material.SetFloat("_Brightness", 3 - 3 * (timer - timerMax * 0.8f) / (timerMax * 0.2f));

                ChargeUp_Particle.Play();
            }

        }
        else
        {
            if (timer >= timerMax * 0.9f) //Impact + Dissolve
            {
                if (timer - Time.deltaTime <= timerMax * 0.9f)
                {
                    GameObject impactobj = Instantiate(Impact_Prefab);
                    impactobj.transform.position = transform.position;
                    impactobj.transform.rotation = transform.rotation;
                    Destroy(impactobj, 0.3f);
                }

                rend_sphere0.material.SetFloat("_DissolveProgress", (timer - timerMax * 0.9f) / (timerMax * 0.1f));
                rend_sphere1.material.SetFloat("_DissolveProgress", (timer - timerMax * 0.9f) / (timerMax * 0.1f));

                rend_torus0.material.SetFloat("_Brightness", 3 - 3 * (timer - timerMax * 0.9f) / (timerMax * 0.1f));
                rend_torus1.material.SetFloat("_Brightness", 3 - 3 * (timer - timerMax * 0.9f) / (timerMax * 0.1f));
                rend_torus2.material.SetFloat("_Brightness", 3 - 3 * (timer - timerMax * 0.9f) / (timerMax * 0.1f));

                transform.localScale = OriginalScale * (1 + 4 * ((timer - timerMax * 0.9f) / (timerMax * 0.1f)));
            }
            else if (timer >= timerMax * 0.7f) //Move
            {
                transform.localPosition += Time.deltaTime * transform.forward * m_speed;
            }
            else //Charging Up
            {
                transform.localScale = OriginalScale * (timer / (timerMax * 0.7f));
            }
        }


    }
}
