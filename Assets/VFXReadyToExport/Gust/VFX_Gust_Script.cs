using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INSTRUCTIONS:
//Spawn the provided object at the user Drumon
//Make this object face the target

public class VFX_Gust_Script : MonoBehaviour
{
    public float timerMax = 3;
    public float timer = 0.0f;

    public bool DeleteOnFinish = true;

    private Vector3 Origin = new Vector3();
    public float m_speed = 1;
    private Vector3 OriginalScale = new Vector3();

    public float TimeToAttack = 0.4f;
    [SerializeField]
    private GameObject Impact_Prefab;

    // Start is called before the first frame update
    void Start()
    {
        Origin = transform.localPosition;
        OriginalScale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);
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
                transform.position = Origin;
            }

        }
        else
        {
            transform.position += transform.forward * Time.deltaTime * m_speed;

            float currentscale = Mathf.Clamp(3 - Mathf.Abs(6 * (timer / timerMax) - 3), 0, 1);
            transform.localScale = OriginalScale * currentscale;

            if (timer - Time.deltaTime <= TimeToAttack && TimeToAttack <= timer)
            {
                GameObject impactobj = Instantiate(Impact_Prefab);
                impactobj.transform.position = transform.position;
                impactobj.transform.rotation = transform.rotation;
                for (int i = 0; i < impactobj.transform.childCount; ++i)
                {
                    impactobj.transform.GetChild(i).localScale *= 2;
                }
                Destroy(impactobj, 0.3f);
            }
            else
            {
                GameObject impactobj = Instantiate(Impact_Prefab);
                impactobj.transform.position = transform.position;
                impactobj.transform.rotation = transform.rotation;
                for (int i = 0; i < impactobj.transform.childCount; ++i)
                {
                    impactobj.transform.GetChild(i).localScale *= 0.3f;
                }
                Destroy(impactobj, 0.3f);
            }

        }
    }
}
