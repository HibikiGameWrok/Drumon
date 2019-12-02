using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INSTRUCTIONS:
//Spawn the provided object at the user Drumon

public class VFX_BlindMist : MonoBehaviour
{
    public float timerMax = 3;
    public float timer = 0.0f;

    public bool DeleteOnFinish = true;

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
            if (DeleteOnFinish)
            {
                Destroy(gameObject);
            }
            else
            {
                timer = 0;

                ParticleSystem[] thelist = GetComponentsInChildren<ParticleSystem>();

                for (int i = 0; i < thelist.Length; ++i)
                {
                    thelist[i].Play();
                }
            }

        }
    }
}
