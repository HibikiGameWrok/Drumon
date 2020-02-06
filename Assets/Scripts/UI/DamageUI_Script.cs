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
    public static void CreateDamageUI(Transform trans,Vector3 offset ,int damage)
    {
        GameObject go = Resources.Load<GameObject>("InsPrefab/DamageUI");

        // 取得できなかったらreturnする
        if (!go)
            return;

        var position = trans.position + offset;

        // 生成する
        go = Instantiate(go, position,Quaternion.identity);

        // 描画先のカメラを設定


        // TextにダメージStringを設定する
        go.GetComponentsInChildren<Text>()[0].text = damage.ToString();
        go.GetComponentsInChildren<Text>()[1].text = damage.ToString();

        // 1秒後に削除する
        Destroy(go, 1.0f);
    }

    public static void CreateWeakUI(Transform trans,Vector3 offset)
    {
        GameObject go = Resources.Load<GameObject>("InsPrefab/WeakUI");

        // 取得できなかったらreturnする
        if (!go)
            return;

        var position = trans.position + offset;

        // 生成する
        go = Instantiate(go, position, Quaternion.identity);

        // 1秒後に削除する
        Destroy(go, 1.0f);
    }
}
