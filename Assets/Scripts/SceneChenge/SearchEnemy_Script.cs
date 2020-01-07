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


    /// <summary>
    /// Destroyされた時の処理
    /// </summary>
    private void OnDestroy()
    {
        // IsHitを伏せる
        m_isHit = false;
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_enemyCreature.EnemyCreatureData = CreateData_Script.Get.CreateData(this.gameObject.tag);
            m_position.Position = this.transform.position;

            // 接触時SEを鳴らす
            AudioManager_Script.Get.PlaySE(SfxType.contact);

            m_isHit = true;

            // 1秒後にdestroyする
            Destroy(gameObject, 1.0f);
        }
    }
}
