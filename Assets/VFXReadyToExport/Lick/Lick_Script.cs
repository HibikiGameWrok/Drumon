using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INSTRUCTIONS:
//Spawn this object in front of the target Drumon

public class Lick_Script : MonoBehaviour
{
    public float timerMax = 1;

    public float timer = 0.0f;

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
        else
        {
            theRenderer.material.SetFloat("_Progress", 0.5f + timer / timerMax);
        }
    }
}
