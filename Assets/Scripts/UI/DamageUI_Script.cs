/*----------------------------------------------------------*/
//  file:      DamageUI_Script.cs                                   |
//				 											                    |
//  brief:    ダメージ表記をするのスクリプト			        |
//															                    |
//  date:	2019.2.5									                |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ダメージ表記するクラス
public class DamageUI_Script : MonoBehaviour
{
    public static void CreateDamageUI(Transform trans, int damage)
    {
        GameObject go = Resources.Load<GameObject>("InsPrefab/DamageUI");

        // 取得できなかったらreturnする
        if (!go)
            return;

       var position = trans.position + new Vector3(0f, 1.5f, -3f);

        // 生成する
        go = Instantiate(go, position,Quaternion.Euler(0f,0f,0f));
        // TextにダメージStringを設定する
        go.GetComponentsInChildren<Text>()[0].text = damage.ToString();
        go.GetComponentsInChildren<Text>()[1].text = damage.ToString();

        // 1秒後に削除する
        Destroy(go, 1.0f);
    }

    public static void CreateWeakUI(Transform trans)
    {
        GameObject go = Resources.Load<GameObject>("InsPrefab/WeakUI");

        // 取得できなかったらreturnする
        if (!go)
            return;

        var position = trans.position + new Vector3(0f, 2f, -3f);

        // 生成する
        go = Instantiate(go, position, Quaternion.Euler(0f, 0f, 0f));

        // 1秒後に削除する
        Destroy(go, 1.0f);
    }
}
