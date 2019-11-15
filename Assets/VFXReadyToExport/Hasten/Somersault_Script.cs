using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INSTRUCTIONS:
//Attach to the Drumon that will use this move
//Only enable the script if you want to start using this move now
//Do not use this with other conflicting VFX scripts

public class Somersault_Script : MonoBehaviour
{
    private Transform transformabove = null;

    public int MaxRotationCount = 3;
    public float Speed = 1500;
    private float CurrentRotation = 0;

    [SerializeField]
    private GameObject somersaultTorus0 = null;
    [SerializeField]
    private GameObject somersaultTorus1 = null;

    private int amountoftorus = 0;

    private bool HasTransformAbove = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (HasTransformAbove == false)
        {
            HasTransformAbove = true;
            transformabove = new GameObject().transform;
            transformabove.position = transform.position + new Vector3(0, 4, 0);
            transform.SetParent(transformabove);
        }

        float addrotation = Speed * Time.deltaTime;

        bool deleteafterrotation = false;

        if (amountoftorus < MaxRotationCount && CurrentRotation >= 90 + 360 * (float)amountoftorus && CurrentRotation - addrotation <= 90 + 360 * (float)amountoftorus)
        {
            ++amountoftorus;
            GameObject torus = Instantiate(somersaultTorus0);
            torus.transform.position = transformabove.position;
            torus.GetComponentInChildren<Renderer>().material.SetColor("_Color", new Color(1, 1, 0.1f));
            torus.GetComponentInChildren<Renderer>().material.SetFloat("_Brightness", 2);
            torus = Instantiate(somersaultTorus1);
            torus.transform.position = transformabove.position;
            torus.GetComponentInChildren<Renderer>().material.SetColor("_Color", new Color(1, 1, 0.1f));
            torus.GetComponentInChildren<Renderer>().material.SetFloat("_Brightness", 2);
        }

        if (addrotation + CurrentRotation >= (float)MaxRotationCount * 360)
        {
            addrotation = (float)MaxRotationCount * 360 - CurrentRotation;
            deleteafterrotation = true;
        }
        
        transformabove.Rotate(-addrotation, 0, 0);
        CurrentRotation += addrotation;

        if (deleteafterrotation)
        {
            amountoftorus = 0;
            CurrentRotation = 0;
            this.enabled = false;

            HasTransformAbove = false;
            transformabove.DetachChildren();
            Destroy(transformabove.gameObject);
        }

    }
}
