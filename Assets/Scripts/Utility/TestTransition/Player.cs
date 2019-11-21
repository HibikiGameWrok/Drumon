using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Player : MonoBehaviour
{
    // 移動速度
    [SerializeField]
    private float m_speed = 2.0f;

    // rigidbodyコンポーネント
    private Rigidbody m_rigidbody;

    // エンカウントしたか
    [SerializeField]
    private BoolReactiveProperty m_isEncounter = new BoolReactiveProperty(false);

    // エンカウントプロパティ
    public IReadOnlyReactiveProperty<bool> IsEncounter => m_isEncounter;

    // Start is called before the first frame update
    void Start()
    {
        // 取得する
        m_rigidbody = GetComponent<Rigidbody>();

        // インプットの検出
        this.FixedUpdateAsObservable()
            .Select(_ => new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")))
            .Subscribe(v => Move(v));

        // 当たり判定
        this.OnCollisionEnterAsObservable()
            .Select(hit => hit.gameObject.tag)
            .Where(tag => tag == "HealDrum")
            .Select(_ => FindObjectOfType<Player>())
            .Subscribe(_ => m_isEncounter.Value = true);
    }


    /// <summary>
    /// 移動関数
    /// </summary>
    /// <param name="v3">移動ベクトル</param>
    private void Move(Vector3 v3)
    {
        m_rigidbody.AddForce(v3.x * m_speed, 0, v3.z * m_speed);
    }


}
