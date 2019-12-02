using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INSTRUCTIONS:
//Attach the provided object as a child of the Drumon using this move
//Set the Drumon using this move as the reference for "ObjectPosition"
//This script will disable itself after finishing
//Do not enable this script with other conflicting VFX

public class VFX_WaterRush_Script : MonoBehaviour
{

    [SerializeField]
    private GameObject WaterRush_Prefab = null;

    [SerializeField]
    private Transform ObjectPosition;

    private Vector3 Origin;

    public float m_speed = 200;

    private float timer = 0.0f;
    private float timerMax = 0.6f;

    public void SetObjectPositionReference(Transform reference)
    {
        ObjectPosition = reference;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        Origin = ObjectPosition.position;
        timer = 0;
        GameObject waterrush = Instantiate(WaterRush_Prefab);
        waterrush.transform.position = transform.position;
        waterrush.transform.rotation = transform.rotation;
        waterrush.transform.Rotate(90, 0, 0);
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
        else
        {
            if (timer >= 0.3f)
            {
                ObjectPosition.position = Vector3.MoveTowards(ObjectPosition.position, Origin, Time.deltaTime * m_speed);
            }
            else
            {
                ObjectPosition.position += transform.forward * Time.deltaTime * m_speed;
            }
        }

    }
}
