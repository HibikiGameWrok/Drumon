using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashModifier_Script : MonoBehaviour
{
    public float timerMax = 1;

    public float timer = 0.0f;

    public float speed = 1;

    private Renderer theRenderer;

    public bool DestroyOnTimeout = true;

    // Start is called before the first frame update
    void Start()
    {
        theRenderer = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timerMax)
        {
            if (DestroyOnTimeout)
                Destroy(gameObject);
            else
                timer = 0;
        }
        else
        {
            //theRenderer.material.SetFloat("_StartCutoff", 1.8f * timer / timerMax - 0.8f);
            theRenderer.material.SetFloat("_StartCutoff", speed * timer / timerMax);
            theRenderer.material.SetFloat("_EndCutoff", speed * timer / timerMax);

            if (timer >= timerMax * 0.5)
            {
                theRenderer.material.SetFloat("_StartCutoffRange", Mathf.Clamp(2 - speed * 2 * timer / timerMax, 0, 0.1f));
                theRenderer.material.SetFloat("_EndCutoffRange", Mathf.Clamp(2 - speed * 2 * timer / timerMax, 0, 0.1f));
            }
            else
            {
                theRenderer.material.SetFloat("_StartCutoffRange", Mathf.Clamp(2 * speed * timer / timerMax, 0, 0.1f));
                theRenderer.material.SetFloat("_EndCutoffRange", Mathf.Clamp(2 * speed * timer / timerMax, 0, 0.1f));
            }

        }

    }
}
