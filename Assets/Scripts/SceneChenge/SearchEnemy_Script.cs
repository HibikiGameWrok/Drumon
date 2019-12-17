using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class SearchEnemy_Script : MonoBehaviour
{
    [SerializeField]
    private EnemyCreature m_enemyCreature;
    [SerializeField]
    PlayerPosition_Script m_position = null;

    // プレイヤーに当たったかどうか
    [SerializeField]
    private bool m_isHit = false;

    // Hitプロパティ
    public bool IsHit => m_isHit;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_enemyCreature.EnemyCreatureData = CreateData_Script.Get.CreateData(this.gameObject.tag);
            m_position.Position = this.transform.position;
            Destroy(gameObject);

            if (this.gameObject.tag == "Tutorial")
            {
                SceneManager.LoadScene("TutorialTestScene", LoadSceneMode.Additive);
            }
            else
            {
                //TransitionManager_Script.StartTransition("BattleScene", LoadSceneMode.Additive);
                //SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
                m_isHit = true;
            }
        }
    }
}
