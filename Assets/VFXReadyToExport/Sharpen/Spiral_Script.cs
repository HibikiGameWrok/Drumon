using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INSTRUCTIONS:
//Attach to the Drumon that will use this move
//Only enable the script if you want to start using this move now
//Do not use this with other conflicting VFX scripts

public class Spiral_Script : MonoBehaviour
{
    private Transform transformabove = null;

    public int MaxRotationCount = 3;
    public float Speed = 2000;
    private float CurrentRotation = 0;

    [SerializeField]
    private GameObject spiral0 = null;

    private int amountofspiral = 0;

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
            transformabove.position = transform.position + new Vector3(0, 0.5f, 0);
            transform.SetParent(transformabove);
        }

        float addrotation = Speed * Time.deltaTime;

        bool deleteafterrotation = false;

        if (amountofspiral < MaxRotationCount && CurrentRotation >= 360 * (float)amountofspiral && CurrentRotation - addrotation <= 90 + 360 * (float)amountofspiral)
        {
            ++amountofspiral;
            GameObject spiral = Instantiate(spiral0);
            spiral.transform.position = transformabove.position;
            Destroy(spiral, 0.5f);
        }

        if (addrotation + CurrentRotation >= (float)MaxRotationCount * 360)
        {
            addrotation = (float)MaxRotationCount * 360 - CurrentRotation;
            deleteafterrotation = true;
        }

        transformabove.Rotate(0, 0, -addrotation);
        CurrentRotation += addrotation;

        if (deleteafterrotation)
        {
            amountofspiral = 0;
            CurrentRotation = 0;
            this.enabled = false;

            HasTransformAbove = false;
            transformabove.DetachChildren();
            Destroy(transformabove.gameObject);
        }
    }
}
