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

    private CharacterController m_enemyController;
    //　目的地
    private Vector3 m_destination;
    //　歩くスピード
    [SerializeField]
    private float m_walkSpeed = 1.0f;
    //　速度
    private Vector3 m_velocity;
    //　移動方向
    private Vector3 m_direction;
    //　到着フラグ
    private bool m_arrived;
    //　スタート位置
    private Vector3 m_startPosition;
    //　待ち時間
    [SerializeField]
    private float m_waitTime = 5.0f;
    //　経過時間
    private float m_elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        m_enemyController = GetComponent<CharacterController>();
        Vector2 randDestination = Random.insideUnitCircle * 8;
        m_startPosition = transform.position;
        m_destination = transform.position;
        CreateRandomPosition();
        m_velocity = Vector3.zero;
        m_arrived = false;
        m_elapsedTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_arrived == false)
        {
            m_velocity = Vector3.zero;
            m_direction = (m_destination - transform.position).normalized;
            transform.LookAt(new Vector3(m_destination.x, transform.position.y, m_destination.z));
            m_velocity = m_direction * m_walkSpeed;

            m_velocity.y += Physics.gravity.y * Time.deltaTime;
            m_enemyController.Move(m_velocity * Time.deltaTime);

            //　目的地に到着したかどうかの判定
            if (Vector3.Distance(transform.position, m_destination) < 0.8f)
            {
                m_arrived = true;
            }
        }
        // 到着していたら
        else
        {
            m_elapsedTime += Time.deltaTime;

            //　待ち時間を越えたら次の目的地を設定
            if (m_elapsedTime > m_waitTime)
            {
                CreateRandomPosition();
                //m_destination = setPosition.GetDestination();
                m_arrived = false;
                m_elapsedTime = 0.0f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("TestBattleScene", LoadSceneMode.Additive);

            m_enemyScript = m_enemyCreature.EnemyCreatureObj.GetComponent<EnemyCreature_Script>();
            m_enemyScript.SetData(m_data);
        }
    }

    // ランダムな位置の作成
    private void CreateRandomPosition()
    {
        //　ランダムなVector2の値を得る
        var randDestination = Random.insideUnitCircle * 8;
        //　現在地にランダムな位置を足して目的地とする
        m_destination = m_startPosition + new Vector3(randDestination.x, 0, randDestination.y);
    }
}
