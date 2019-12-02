using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_WaterImpact_Script : MonoBehaviour
{
    private float timer = 0;
    public float timerMax = 1.35f;

    [SerializeField]
    private Renderer rend_WaterImpact;

    public float TransitionSpeed = 3;

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
            Destroy(gameObject);
        }
        else
        {
            float abstime = Mathf.Clamp(TransitionSpeed - Mathf.Abs(TransitionSpeed * 2 * timer / timerMax - TransitionSpeed), 0, 0.5f);

            rend_WaterImpact.material.SetFloat("_Progress", abstime);
        }
    }
}
