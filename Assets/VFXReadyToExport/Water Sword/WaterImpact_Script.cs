using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterImpact_Script : MonoBehaviour
{
    public float timerMax = 0.5f;
    private float timer = 0.0f;

    [SerializeField]
    private Renderer theRenderer = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timerMax) Destroy(gameObject);
        else if (timer <= timerMax * 0.5f)
        {
            theRenderer.material.SetFloat("_Progress", timer / timerMax);
        }
        else
        {
            var thescale = theRenderer.transform.parent.localScale;
            thescale.y = 1 - (timer - timerMax * 0.5f) / (timerMax * 0.5f);
            theRenderer.transform.parent.localScale = thescale;
        }
    }
}