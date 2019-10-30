using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildeCollider_Script : MonoBehaviour
{
    // 攻撃ドラム(親)
    private AttackDrum_Script m_attackDrum;

    // Start is called before the first frame update
    void Start()
    {
        GameObject objColliderParent = this.gameObject.transform.parent.gameObject;
        m_attackDrum = objColliderParent.GetComponent<AttackDrum_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 子の当たり判定
    void OnTriggerEnter(Collider collider)
    {
        // スティックに当たったら
        if (collider.tag == "Stick")
        {
            // 攻撃ドラムの内側だったら
            //if (this.tag == "AttackInDrum")
            //{
            //    // 親の当たり判定
            //    m_attackDrum.InDrumTrigger(collider);
            //}
            //// 攻撃ドラムの外側だったら
            //else if (this.tag == "AttackOutDrum")
            //{
            //    // 親の当たり判定
            //    m_attackDrum.OutDrumTrigger(collider);
            //}

            // 親の当たり判定
            m_attackDrum.OnTriggerEnter(collider);
        }
    }
}
