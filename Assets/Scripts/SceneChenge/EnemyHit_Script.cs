using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHit_Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //--------------------------
        if (Input.GetKeyDown(KeyCode.A))
        {
            SceneManager.LoadScene("LoadSceneTest2", LoadSceneMode.Additive);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.UnloadSceneAsync("LoadSceneTest2");
        }
        //--------------------------
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("LoadSceneTest2", LoadSceneMode.Additive);
        }
    }
}
