using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeBarrage_Script : MonoBehaviour
{

    private float timer = 0.0f;
    public float timerMax = 0.7f;

    private float particletimer = 0;
    public int particleAmount = 50;
    [SerializeField]
    private GameObject particleObject = null;

    public float range = 1;

    public bool DeleteOnFinish = true;

    public bool InitialRotate = false;

    public float BreathRange = 20;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer < 0) return;

        if (timer >= timerMax)
        {
            if (DeleteOnFinish)
            {
                Destroy(gameObject);
            }
            else
            {
                timer = -0.2f;
                particletimer = 0;
            }

        }
        else
        {
            particletimer += Time.deltaTime;
            int limiter = 15;
            while (particletimer >= timerMax / (float)particleAmount && limiter > 0)
            {
                limiter--;
                particletimer -= timerMax / (float)particleAmount;

                GameObject particle = Instantiate(particleObject);
                particle.transform.SetParent(transform);
                particle.transform.localPosition = new Vector3(Random.Range(-(float)range, (float)range), 0, Random.Range(-(float)range, (float)range));
                particle.transform.rotation = new Quaternion();
                particle.transform.SetParent(null);

                if (InitialRotate)
                {
                    particle.transform.Rotate(Random.Range(-BreathRange, BreathRange), Random.Range(-BreathRange, BreathRange), Random.Range(-BreathRange, BreathRange));
                }

            }

        }

    }
}
