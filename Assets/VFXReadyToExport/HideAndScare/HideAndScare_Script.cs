using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndScare_Script : MonoBehaviour
{
    private float timer = 0.0f;
    public float timerMax = 4.0f;

    [SerializeField]
    private Transform ObjectReference = null;

    private Vector3 Origin;

    public float m_distance = 5;
    public float m_downdistance = 4;

    [SerializeField]
    private GameObject Portal_Prefab = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timerMax)
        {

            timer = 0;

            ObjectReference.transform.position = Origin;

            this.enabled = false;

        }
        else
        {
            if (timerMax * 0.8f <= timer)
            {
                if (timer - Time.deltaTime <= timerMax * 0.8f)
                {
                    transform.position -= m_distance * (-transform.forward);
                }
                float InnerProgress = (timer - timerMax * 0.8f) / (timerMax * (1 - 0.8f));

                transform.position += m_downdistance * transform.up * Time.deltaTime / (timerMax * 0.2f);

            }
            else if (timerMax * 0.6f <= timer)
            {
                if (timer - Time.deltaTime <= timerMax * 0.6f)
                {

                }
                float InnerProgress = (timer - timerMax * 0.6f) / (timerMax * (0.8f - 0.6f));

                transform.position -= m_downdistance * transform.up * Time.deltaTime / (timerMax * 0.2f);

            }
            else if (timerMax * 0.4f <= timer)
            {
                if (timer - Time.deltaTime <= timerMax * 0.4f)
                {
                    
                }
                float InnerProgress = (timer - timerMax * 0.4f) / (timerMax * (0.6f - 0.4f));

               

            }
            else if (timerMax * 0.2f <= timer)
            {
                if (timer - Time.deltaTime <= timerMax * 0.2f)
                {
                    transform.position += m_distance * (-transform.forward);
                }
                float InnerProgress = (timer - timerMax * 0.2f) / (timerMax * (0.4f - 0.2f));

                transform.position += m_downdistance * transform.up * Time.deltaTime / (timerMax * 0.2f);

            }
            else
            {
                if (timer - Time.deltaTime <= 0.0)
                {
                    Origin = transform.position;

                    GameObject portal;
                    portal = Instantiate(Portal_Prefab);
                    portal.transform.position = Origin - transform.up * (m_downdistance / 2);
                    //Destroy(portal, timerMax);
                    portal = Instantiate(Portal_Prefab);
                    portal.transform.position = Origin - transform.up * (m_downdistance / 2) + (-transform.forward * m_distance);
                    //Destroy(portal, timerMax);
                }

                float InnerProgress = (timer) / (timerMax * 0.2f);

                transform.position -= m_downdistance * transform.up * Time.deltaTime / (timerMax * 0.2f);

            }

        }


    }
}
