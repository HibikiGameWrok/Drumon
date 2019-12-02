using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INSTRUCTIONS:
//Spawn the provided object at the target Drumon

public class DarkBind_Script : MonoBehaviour
{
    public float timerMax = 1;
    public float timer = 0.0f;

    private List<GameObject> childtoruses = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            childtoruses.Add(transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timerMax)
        {
            timer = 0;
            //Destroy(gameObject);
        }

        float scaleval = 1;
        if (timer >= timerMax * 0.5f)
        {
            scaleval = 1 - (timer - timerMax * 0.5f) / (timerMax * 0.5f);
        }
        else
        {
            scaleval = (timer) / (timerMax * 0.5f);
        }

        for (int i = 0; i < childtoruses.Count; ++i)
        {
            childtoruses[i].transform.localScale = new Vector3(scaleval, 1, scaleval);
        }

    }
}
