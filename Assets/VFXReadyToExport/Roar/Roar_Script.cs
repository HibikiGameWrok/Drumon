using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INSTRUCTIONS:
//Spawn the provided object in front of the Drumon using this move

public class Roar_Script : MonoBehaviour
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
            //timer = 0;
            Destroy(gameObject);
        }

        theRenderer.material.SetFloat("_StartCutoff", Mathf.Clamp(0.9f * (1 - 3 * (timer / timerMax)) + 0.1f, 0.1f, 1));

        theRenderer.material.SetFloat("_EndCutoff", Mathf.Clamp(1 - (timer / timerMax), 0, 0.5f));
        //theRenderer.material.SetFloat("_EndCutoffRange", Mathf.Clamp(0.4f * (1 - (timer / timerMax)) - 0.2f, 0, 0.1f));

    }
}
