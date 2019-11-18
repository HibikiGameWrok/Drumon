using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[Important] Making a Catch animation for each 3D model

//INSTRUCTIONS:
//Each CatchAnimationManager should have 2 Models with the respective Shader Materials
//"TargetPosition" is the global position that the model goes to
//"CurvedDistortion" is the displacement of the animation to make the animation curved

public class CatchAnimation_Script : MonoBehaviour
{
    public Vector3 TargetPosition = new Vector3(0, 0, 10);
    public Vector3 CurvedDistortion = new Vector3(3, 8, 0);

    private float timer = 0;
    public float timerMax = 0.5f;

    private Renderer[] rendlist = null;

    public bool DestroyOnFinish = true;

    // Start is called before the first frame update
    void Start()
    {
        rendlist = transform.GetComponentsInChildren<Renderer>();

        for (int i = 0; i < rendlist.Length; ++i)
        {
            for (int j = 0; j < rendlist[i].materials.Length; ++j)
            {
                rendlist[i].materials[j].SetVector("_TargetPos", TargetPosition);
                rendlist[i].materials[j].SetVector("_CurvedDistortion", CurvedDistortion);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timerMax)
        {
            if (DestroyOnFinish)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                timer = 0;
            }
        }



        for (int i = 0; i < rendlist.Length; ++i)
        {
            for (int j = 0; j < rendlist[i].materials.Length; ++j)
            {
                rendlist[i].materials[j].SetVector("_TargetPos", TargetPosition);
                rendlist[i].materials[j].SetVector("_CurvedDistortion", CurvedDistortion);
                rendlist[i].materials[j].SetFloat("_DissolveProgress", 1 - Mathf.Clamp(3 * (timer / timerMax), 0, 1));
                rendlist[i].materials[j].SetFloat("_Progress", Mathf.Clamp(3 * (timer / timerMax) - 2, 0, 1));
            }
        }


    }
}
