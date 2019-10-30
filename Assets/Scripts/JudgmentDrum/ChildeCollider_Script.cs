using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildeCollider_Script : MonoBehaviour
{
    // 攻撃ドラム(親)
    private AttackDrum_Script m_attackDrum;
    // 選択ドラム(親)
    private SwitchDrum_Script m_switchDrum;

    // Start is called before the first frame update
    void Start()
    {
        GameObject objColliderParent = this.gameObject.transform.parent.gameObject;
        m_attackDrum = objColliderParent.GetComponent<AttackDrum_Script>();
        m_switchDrum = objColliderParent.GetComponent<SwitchDrum_Script>();
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
            // 攻撃ドラムだったら
            if (this.tag == "AttackInDrum" || this.tag == "AttackOutDrum")
            {
                // 親の当たり判定
                m_attackDrum.OnTriggerEnter(collider);
            }
            // 選択ドラムだったら
            else if (this.tag == "SwitchInDrum" || this.tag == "SwitchOutDrum")
            {
                // 親の当たり判定
                m_switchDrum.OnTriggerEnter(collider);
            }
        }
    }
}
