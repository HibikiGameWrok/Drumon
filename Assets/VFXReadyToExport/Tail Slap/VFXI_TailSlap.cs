using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INSTRUCTIONS:
//Spawn the provided object at the target

public class VFXI_TailSlap : MonoBehaviour
{
    public double timerMax = 0.5;
    private double timer = 0;

    [SerializeField]
    private GameObject Slash1_Prefab = null;
    [SerializeField]
    private GameObject Slash2_Prefab = null;
    [SerializeField]
    private GameObject Impact_Prefab = null;


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
            timer -= timerMax;
        }
        else if (timer >= 0.22 && timer - Time.deltaTime < 0.22)
        {
            GameObject impact = Instantiate(Impact_Prefab);
            impact.transform.position = transform.position + transform.forward * 2.5f;
            impact.transform.rotation = transform.rotation;
            Destroy(impact, 0.3f);
        }
        else if (timer >= 0.11 && timer - Time.deltaTime < 0.11)
        {
            GameObject slash = Instantiate(Slash2_Prefab);
            slash.transform.position = transform.position;
            slash.transform.SetParent(transform);
            slash.transform.rotation = new Quaternion();
            slash.transform.SetParent(null);
            slash.transform.Rotate(0, -90, 0);
        }
        else if (timer >= 0.1 && timer - Time.deltaTime < 0.1)
        {
            GameObject slash = Instantiate(Slash1_Prefab);
            slash.transform.position = transform.position;
            slash.transform.SetParent(transform);
            slash.transform.rotation = new Quaternion();
            slash.transform.SetParent(null);
            slash.transform.Rotate(0, -90, 0);
        }
    }
}

