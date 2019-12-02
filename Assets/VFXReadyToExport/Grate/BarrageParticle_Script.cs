using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrageParticle_Script : MonoBehaviour
{

    private float timer = 0.0f;
    public float timerMax = 0.5f;

    public float speed = 10;

    private Renderer theRenderer;

    [SerializeField]
    private GameObject Impact_Prefab;

    // Start is called before the first frame update
    void Start()
    {
        theRenderer = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timerMax)
        {
            GameObject impactobj = Instantiate(Impact_Prefab);
            impactobj.transform.position = transform.position;
            impactobj.transform.rotation = transform.rotation;
            impactobj.transform.Rotate(-90, 0, 0);
            Destroy(impactobj, 0.3f);
            Destroy(gameObject);
        }


        transform.position += speed * transform.up * Time.deltaTime;

        if (timer >= 0.8f * timerMax)
        {
            theRenderer.material.SetFloat("_DissolveProgress", (timer - 0.8f * timerMax) / (timerMax - 0.8f * timerMax));
        }

    }
}
