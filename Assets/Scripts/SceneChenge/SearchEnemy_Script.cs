using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SearchEnemy_Script : MonoBehaviour
{
    [SerializeField]
    private CharactorData m_data = null;
    [SerializeField]
    private EnemyCreature m_enemyCreature;
    private EnemyCreature_Script m_enemyScript;

    //　歩くスピード
    private float m_speed = 1.0f;
    // 回転速度
    private float m_rotationSpeed = 1.0f;
    // 目標位置の範囲
    private float m_posRange = 10.0f;
    // 目標位置の位置ベクトル
    private Vector3 m_targetPos;
    // 近づいたら目標位置を変える
    private float m_changeTarget = 5.0f;
    // 目標位置までの距離
    private float m_targetDistance;

    // Start is called before the first frame update
    void Start()
    {
        m_targetPos = GetRandomPosition(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        m_targetDistance = Vector3.SqrMagnitude(transform.position - m_targetPos);
        MoveEnemy();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_enemyScript = m_enemyCreature.EnemyCreatureObj.GetComponent<EnemyCreature_Script>();
            m_enemyScript.SetData(m_data);
            Destroy(gameObject);

            SceneManager.LoadScene("TestBattleScene", LoadSceneMode.Additive);
        }
    }

    // ランダムな位置の作成
    Vector3 GetRandomPosition(Vector3 currentpos)
    {
        return new Vector3(Random.Range(-m_posRange + currentpos.x, m_posRange + currentpos.x), transform.position.y, Random.Range(-m_posRange + currentpos.z, m_posRange + currentpos.z));
    }

    // 移動
    void MoveEnemy()
    {
        if (m_targetDistance < m_changeTarget)
        {
            m_targetPos = GetRandomPosition(transform.position);
        }

        Quaternion targetRotation = Quaternion.LookRotation(m_targetPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_rotationSpeed);
        transform.Translate(Vector3.forward * m_speed * Time.deltaTime);
    }
}
