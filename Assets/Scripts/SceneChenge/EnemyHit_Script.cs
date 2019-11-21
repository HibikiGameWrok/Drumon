using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHit_Script : MonoBehaviour
{
    [SerializeField]
    private CharactorData m_data = null;
    private GameObject m_enemy;
    private EnemyCreature_Script m_enemyScript;

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
            m_enemy = GameObject.Find("EnemyCreature");
            m_enemyScript = m_enemy.GetComponent<EnemyCreature_Script>();
            m_enemyScript.SetData(m_data);
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

    //public CharactorData GetData()
    //{
    //    return this.m_data;
    //}
}
