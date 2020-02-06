using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_LeafShower_Script : MonoBehaviour
{

    private float timer = 0;
    public float timerMax = 2;

    public bool DestroyOnFinish = true;

    [SerializeField]
    private Renderer rend_Tornado = new Renderer();
    [SerializeField]
    private Renderer rend_Leaves0 = new Renderer();
    [SerializeField]
    private Renderer rend_Leaves1 = new Renderer();
    [SerializeField]
    private Renderer rend_Leaves2 = new Renderer();
    [SerializeField]
    private Transform Barrage_Transform = null;
    [SerializeField]
    private GameObject Barrage_Prefab = null;
    [SerializeField]
    private ParticleSystem Burst = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timerMax) //End
        {
            if (DestroyOnFinish)
            {
                Destroy(gameObject);
            }
            else
            {
                timer = 0;
                Burst.Play();
            }
        }
        else
        {
            if (timer >= timerMax * 0.75f) //fade out
            {
                rend_Tornado.material.SetFloat("_TestCutoffVar", 1 - (timer - timerMax * 0.75f) / (timerMax - timerMax * 0.75f));
                rend_Leaves0.material.SetFloat("_Brightness", 1 - (timer - timerMax * 0.75f) / (timerMax - timerMax * 0.75f));
                rend_Leaves1.material.SetFloat("_Brightness", 1 - (timer - timerMax * 0.75f) / (timerMax - timerMax * 0.75f));
                rend_Leaves2.material.SetFloat("_Brightness", 1 - (timer - timerMax * 0.75f) / (timerMax - timerMax * 0.75f));
            }
            else if (timer >= timerMax * 0.10f)
            {
                //if (timer - Time.deltaTime <= timerMax * 0.10f)
                //{
                //    GameObject barrageobj = Instantiate(Barrage_Prefab, Barrage_Transform);
                //    barrageobj.transform.localPosition = new Vector3(0, 0, 0);
                //}
            }
            else if (timer >= timerMax * 0.07f) //barrage
            {
                if (timer - Time.deltaTime <= timerMax * 0.07f)
                {
                    rend_Tornado.material.SetFloat("_TestCutoffVar", 1);
                    rend_Leaves0.material.SetFloat("_Brightness", 1);
                    rend_Leaves1.material.SetFloat("_Brightness", 1);
                    rend_Leaves2.material.SetFloat("_Brightness", 1);

                    GameObject barrageobj = Instantiate(Barrage_Prefab, Barrage_Transform);
                    barrageobj.transform.localPosition = new Vector3(0, 0, 0);
                }
            }
            else //fade in
            {
                rend_Tornado.material.SetFloat("_TestCutoffVar", (timer) / (timerMax * 0.07f));
                rend_Leaves0.material.SetFloat("_Brightness", (timer) / (timerMax * 0.07f));
                rend_Leaves1.material.SetFloat("_Brightness", (timer) / (timerMax * 0.07f));
                rend_Leaves2.material.SetFloat("_Brightness", (timer) / (timerMax * 0.07f));
            }
        }

    }
}
