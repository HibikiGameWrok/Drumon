using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHit_Script : MonoBehaviour
{
    [SerializeField]
    private CharactorData m_data = null;
    [SerializeField]
    private EnemyCreature m_enemyCreature;
    private EnemyCreature_Script m_enemyScript;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //--------------------------
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    SceneManager.LoadScene("LoadSceneTest2", LoadSceneMode.Additive);
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    SceneManager.UnloadSceneAsync("LoadSceneTest2");
        //}
        //--------------------------
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //SceneManager.LoadScene("TestBattleScene", LoadSceneMode.Additive);
            // LoadSceneModeで現在のシーンに展開するか決める
            TransitionManager_Script.StartTransition("TestBattleScene");

            m_enemyScript = m_enemyCreature.EnemyCreatureObj.GetComponent<EnemyCreature_Script>();
            m_enemyScript.SetData(m_data);
        }
    }
}
