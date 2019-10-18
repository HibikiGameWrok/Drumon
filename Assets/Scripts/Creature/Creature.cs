//
//      FileName @ Creture.cs
//
//      Creater  @ Hibiki Yoshiyasu
//
//      Day      @ 2019 / 10 / 16      
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    // キャラクターデータ
    [SerializeField]
    private CharactorData m_charaData = null;

    // データのプロパティ
    public CharactorData CharaData
    {
        get { return m_charaData; }
    }


    // 相手へ攻撃する関数
    public void Attack(Creature target)
    {
        // 自身の攻撃力
        int mCharaAtk = this.m_charaData.Atk;
        // ターゲットの防御力
        int tCharaDef = target.CharaData.Def;
        // 計算後のダメージを初期化
        int calcuDamage = 0;

        // ダメージ計算(ドラクエ方式)
        calcuDamage = (mCharaAtk / 2) - (tCharaDef / 4);

        // 弱点計算
        calcuDamage = target.WeakChack(target.CharaData.Elem, calcuDamage);

        // 相手のダメージ処理関数を呼ぶ
        target.Damage(calcuDamage);
    }

    // 弱点判断
    private int WeakChack(CharactorData.ELEM targetElem, int damage)
    {
        // 属性計算
        int calcuElem = this.m_charaData.Elem - targetElem;

        // 絶対値
        if (Mathf.Abs(calcuElem) == 1)
        {
            if (calcuElem > 0)
            {
                // ダメージ半減
                damage = damage / 2;
            }
            else
            {
                // ダメージ２倍
                damage = damage * 2;
            }
        }

        // 弱点倍率計算したダメージを返す
        return damage;
    }

    // 相手の攻撃から受けるダメージ
    public void Damage(int damage)
    {
        // 自身のHPに反映させる
        this.m_charaData.Hp = damage;
    }
}
