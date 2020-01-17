using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_Script : MonoBehaviour
{
    private float timer = 0.0f;
    public float timerMax = 4.0f;

    public bool DeleteOnFinish = true;

    [SerializeField]
    private Transform trans_PortalWindow = null;
    [SerializeField]
    private Transform trans_PortalInside = null;
    [SerializeField]
    private Transform trans_PortalRingSide = null;
    [SerializeField]
    private Transform trans_PortalRingUp = null;
    [SerializeField]
    private Transform trans_PortalRingSwirl = null;

    private Transform[] swirls = null;

    // Start is called before the first frame update
    void Start()
    {
        swirls = trans_PortalRingSwirl.GetComponentsInChildren<Transform>();

        trans_PortalWindow.transform.localScale = new Vector3(0, 0, 0);
        trans_PortalInside.transform.localScale = new Vector3(0, 0, 0);
        trans_PortalRingSide.transform.localScale = new Vector3(0, 0, 0);
        trans_PortalRingUp.transform.localScale = new Vector3(0, 0, 0);
        
        for (int i = 0; i < swirls.Length; ++i)
        {
            swirls[i].localScale = new Vector3(0, 0, 0);
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
                trans_PortalWindow.transform.localScale = new Vector3(0, 0, 0);
                trans_PortalInside.transform.localScale = new Vector3(0, 0, 0);
                trans_PortalRingSide.transform.localScale = new Vector3(0, 0, 0);
                trans_PortalRingUp.transform.localScale = new Vector3(0, 0, 0);
            }
        }
        else
        {
            if (timerMax * 0.9f <= timer)
            {
                if (timer - Time.deltaTime <= timerMax * 0.9f)
                {

                }
                float InnerProgress = (timer - timerMax * 0.9f) / (timerMax * (1 - 0.9f));

                trans_PortalWindow.transform.localScale = new Vector3(1, 1, 1) * (1 - InnerProgress);
                trans_PortalInside.transform.localScale = new Vector3(1.1f, 1.0f, 1.1f) * (1 - InnerProgress);
                trans_PortalRingSide.transform.localScale = new Vector3(1, 0.7f, 1) * (1 - InnerProgress);
                trans_PortalRingUp.transform.localScale = new Vector3(1, 0.7f, 1) * (1 - InnerProgress);

                for (int i = 0; i < swirls.Length; ++i)
                {
                    swirls[i].localScale = new Vector3(1, 0.7f, 1) * (1 - InnerProgress);
                }

            }
            else if (timerMax * 0.1f <= timer)
            {
                if (timer - Time.deltaTime <= timerMax * 0.1f)
                {
                    //trans_PortalWindow.transform.localScale = new Vector3(1, 1, 1);
                    //trans_PortalInside.transform.localScale = new Vector3(1.1f, 1.0f, 1.1f);
                    //trans_PortalRingSide.transform.localScale = new Vector3(1, 0.7f, 1);
                    //trans_PortalRingUp.transform.localScale = new Vector3(1, 0.7f, 1);

                    for (int i = 0; i < swirls.Length; ++i)
                    {
                        swirls[i].localScale = new Vector3(1, 0.7f, 1);
                    }

                }
                float InnerProgress = (timer - timerMax * 0.1f) / (timerMax * (0.9f - 0.1f));

                float tempoffset = (1 + 0.03f * Mathf.Sin(3 * InnerProgress * Mathf.PI * 2));

                trans_PortalWindow.transform.localScale = new Vector3(1, 1, 1) * tempoffset;
                trans_PortalInside.transform.localScale = new Vector3(1.1f, 1.0f, 1.1f) * tempoffset;
                trans_PortalRingSide.transform.localScale = new Vector3(1, 0.7f, 1) * tempoffset;
                trans_PortalRingUp.transform.localScale = new Vector3(1, 0.7f, 1) * tempoffset;

                for (int i = 0; i < swirls.Length; ++i)
                {
                    swirls[i].localScale = new Vector3(1, 0.7f, 1) * tempoffset;
                }

            }
            else
            {
                float InnerProgress = (timer) / (timerMax * 0.1f);

                trans_PortalWindow.transform.localScale = new Vector3(1, 1, 1) * InnerProgress;
                trans_PortalInside.transform.localScale = new Vector3(1.1f, 1.0f, 1.1f) * InnerProgress;
                trans_PortalRingSide.transform.localScale = new Vector3(1, 0.7f, 1) * InnerProgress;
                trans_PortalRingUp.transform.localScale = new Vector3(1, 0.7f, 1) * InnerProgress;

                for (int i = 0; i < swirls.Length; ++i)
                {
                    swirls[i].localScale = new Vector3(1, 0.7f, 1) * InnerProgress;
                }

            }

        }

    }
}
