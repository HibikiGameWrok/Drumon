using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreaksModifier_Script : MonoBehaviour
{

    private float timer = 0.0f;
    public float timerMax = 0.25f;

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

        theRenderer.material.SetFloat("_StartCutoff", Mathf.Clamp(1 - 3 * 0.65f * (timer / timerMax), 0.35f, 1));
        theRenderer.material.SetFloat("_StartCutoffRange", Mathf.Clamp(3 * 0.35f * (timer / timerMax), 0, 0.35f));

        theRenderer.material.SetFloat("_EndCutoff", Mathf.Clamp(2.5f * (1 - timer / timerMax), 0, 1.0f));
        theRenderer.material.SetFloat("_EndCutoffRange", Mathf.Clamp(2.5f * 0.2f * (1 - timer / timerMax), 0, 0.2f));

        transform.position += transform.up * 64 * Time.deltaTime;
    }
}
