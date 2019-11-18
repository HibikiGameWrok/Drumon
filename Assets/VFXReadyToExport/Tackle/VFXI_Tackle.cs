using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INSTRUCTIONS:
//Attach the provided object as a child of the Drumon using this move
//Set the Drumon using this move as the reference for "ObjectPosition"
//This script will disable itself after finishing
//Do not enable this script with other conflicting VFX

public class VFXI_Tackle : MonoBehaviour
{

    [SerializeField]
    private GameObject Streaks_Prefab = null;
    [SerializeField]
    private GameObject Streaks1_Prefab = null;
    [SerializeField]
    private GameObject Impact_Prefab = null;

    [SerializeField]
    private Transform ObjectPosition;

    private Vector3 Origin;
    private Vector3 Destination;

    private float timer = 0.0f;
    public float timerMax = 0.5f;

    public void SetObjectPositionReference(Transform reference)
    {
        ObjectPosition = reference;
    }

    // Start is called before the first frame update
    void Start()
    {
        //ObjectPosition = transform.parent;
        Origin = ObjectPosition.position;
        Destination = transform.position + transform.forward * 15.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timerMax)
        {
            timer -= timerMax;
            ObjectPosition.position = Origin;
            this.enabled = false;
        }
        else if (timer >= 0.25 && timer - Time.deltaTime < 0.25)
        {
            GameObject impact = Instantiate(Impact_Prefab);
            impact.transform.position = Origin + transform.forward * 19.0f;
            Destroy(impact, 0.3f);
        }
        else if (timer >= 0.04 && timer - Time.deltaTime < 0.04)
        {
            Instantiate(Streaks_Prefab).transform.position = Origin;

            if (Streaks1_Prefab != null)
                Instantiate(Streaks1_Prefab).transform.position = Origin;

        }

        if (timer >= 0.25)
        {
            ObjectPosition.position = Vector3.MoveTowards(ObjectPosition.position, Origin, Time.deltaTime * 15 / 0.2f);
        }
        else if (timer >= 0.05)
        {
            ObjectPosition.position = Vector3.MoveTowards(ObjectPosition.position, Destination, Time.deltaTime * 15 / 0.2f);
        }

    }
}
