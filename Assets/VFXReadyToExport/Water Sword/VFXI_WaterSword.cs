using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INSTRUCTIONS:
//Spawn this at the target Drumon
//Only rotate this object in the y-axis

public class VFXI_WaterSword : MonoBehaviour
{

    [SerializeField]
    private GameObject Sword_Prefab;
    [SerializeField]
    private GameObject Slash1_Prefab;
    [SerializeField]
    private GameObject Slash2_Prefab;
    [SerializeField]
    private GameObject Impact_Prefab;

    private float timer = 0.0f;
    public float timerMax = 0.7f;

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
            timer = 0;
        }
        else if (timer >= 0.20 + 0.1 && timer - Time.deltaTime < 0.20 + 0.1)
        {
            GameObject impact = Instantiate(Impact_Prefab);
            impact.transform.position = transform.position + transform.forward * 5;
        }
        else if (timer >= 0.11 + 0.1 && timer - Time.deltaTime < 0.11 + 0.1)
        {
            GameObject slash = Instantiate(Slash2_Prefab);
            slash.transform.position = transform.position;
            slash.transform.Rotate(new Vector3(1, 0, 0), transform.eulerAngles.y);
        }
        else if (timer >= 0.1 + 0.1 && timer - Time.deltaTime < 0.1 + 0.1)
        {
            GameObject slash = Instantiate(Slash1_Prefab);
            slash.transform.position = transform.position;
            slash.transform.Rotate(new Vector3(1, 0, 0), transform.eulerAngles.y);
        }
        else if (timer >= 0.01 && timer - Time.deltaTime < 0.01)
        {
            GameObject sword = Instantiate(Sword_Prefab);
            sword.transform.position = transform.position;
            sword.transform.Rotate(new Vector3(0, 0, 1), transform.eulerAngles.y);
        }
    }
}
