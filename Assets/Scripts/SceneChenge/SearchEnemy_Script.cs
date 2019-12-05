using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class SearchEnemy_Script : MonoBehaviour
{
    [SerializeField]
    private EnemyCreature m_enemyCreature;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_enemyCreature.EnemyCreatureData = CreateData_Script.Get.CreateData(this.gameObject.tag);
            Destroy(gameObject);

            if (this.gameObject.tag == "Tutorial")
            {
                SceneManager.LoadScene("TutorialTestScene", LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
            }
        }
    }
}
