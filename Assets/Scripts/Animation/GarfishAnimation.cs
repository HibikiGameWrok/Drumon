using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarfishAnimation : MonoBehaviour
{
    Animator anim = null;
    float length = 0.0f;
    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);

        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetBool("IsDeath", true);
            length = animState.length;
            timer = 0.0f;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetBool("IsAttack", true);
            length = animState.length;
            timer = 0.0f;
        }

        if(length != 0.0f)
        {
            timer += Time.deltaTime;
            if (length < timer)
                if (animState.IsName("Death"))
                {
                    Destroy(this.gameObject);
                }
                else if (animState.IsName("Attack"))
                {
                    anim.SetBool("IsAttack", false);
                }
        }
    }
}
