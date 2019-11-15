using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordGlow_Script : MonoBehaviour
{
    private float timer = 0.0f;
    public float timerMax = 0.7f;

    private Renderer theRenderer;

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
            Destroy(gameObject);
        }
        else
        {
            theRenderer.material.SetFloat("_Brightness", Mathf.Clamp(5 * (1 - Mathf.Abs(timer * 2 - timerMax) / timerMax), 0, 1));

            if (timer <= timerMax * 0.25 && timer >= timerMax * 0.1)
            {
                transform.Rotate(0, -1500 * Time.deltaTime, 0);
            }
        }

    }
}
