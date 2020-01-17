using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake_Script : MonoBehaviour
{

    [SerializeField]
    private GameObject EarthRock_Prefab = null;

    public int amount = 12;

    public float range = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {

        for (int i = 0; i < amount; ++i)
        {
            float randomangle = Random.Range(0.0f, 44/7);

            Instantiate(EarthRock_Prefab).transform.position = transform.position + range * new Vector3(Mathf.Cos(randomangle), 0, Mathf.Sin(randomangle));

        }

        this.enabled = false;
    }

}
