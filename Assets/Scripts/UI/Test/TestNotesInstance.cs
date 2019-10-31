using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNotesInstance : MonoBehaviour
{
    // プレハブの名前を定数化
    private const string NOTES_NAME_PATH = "InsPrefab/NotePrefab";

    // ノーツ生成の最大値
    private const int MAX_NOTES = 8;

    // 生成するノーツの種類
    private enum NOTES_TYPE : int
    {
        NONE = 0,
        DO_NOTE,
        DON_NOTE,
        KA_NOTE,
        KAN_NOTE,
        NOTES_COUNT,
    }

    // ノーツプレハブを保持
    private GameObject m_notesPrefab = null;

    // 生成された数を保持
    private int m_countnNotes = 0; 

    // 生成座標を保持
    private Vector3 m_instancePos;

    // Start is called before the first frame update
    void Start()
    {
        // 親の座標を基準とする
        m_instancePos = this.transform.position;

        for (int j = 0; j < MAX_NOTES; j++)
        {
            // 全４種分のノーツを４つ生成
            for (int i = 1; i < (int)NOTES_TYPE.NOTES_COUNT; i++)
            {
                InstanceNotes(i);
            }
        }
    }
    
    // ノーツプレハブを番号によって生成する関数
    public void InstanceNotes(int num)
    {
        // 最大値よりも少なければ
        if (m_countnNotes < MAX_NOTES)
        {
            // プレハブ生成
            m_notesPrefab = Instantiate(
                SetLodeNotesPrefab(num),
                new Vector3(SetInsPosX(m_countnNotes), SetInsPosY(num), m_instancePos.z),
                Quaternion.identity) as GameObject;

            // カウントアップ
            UPCountNotes();

            // 親子関係を設定
            m_notesPrefab.transform.parent = this.transform;

            // 親のScaleに合わせてプレハブの大きさを変える
            m_notesPrefab.transform.localScale = m_notesPrefab.transform.lossyScale * this.transform.localScale.x;
        }
    }

    
    // 生成するノーツプレハブの番号を設定し返す関数
    private GameObject SetLodeNotesPrefab(int num)
    {
        // ResourcesファイルからPrefabデータを設定(末尾の番号によってデータが変わる)
        m_notesPrefab = (GameObject)Resources.Load(NOTES_NAME_PATH + num);

        return m_notesPrefab;
    }


    // 生成する度にX座標を「＋」方向へずらす処理　(カウント参照)
    private float SetInsPosX(int notesCount)
    {
        // 生成する度にずらす
        float addValue = (0.237f * notesCount) * this.transform.parent.localScale.x;

        // X座標へずらす計算
        m_instancePos.x = this.transform.position.x + addValue;

        // 計算した変数を返す
        return m_instancePos.x;
    }


    // ノーツの種類によって生成座標のY座標を設定　(ノーツの種類参照)
    private float SetInsPosY(int notesNum)
    {
        // 基準値からY座標を下にずらす為の変数
        float subValue = 0.092f * this.transform.parent.localScale.y;

        // 叩かれ方によってY座標を変更する
        switch(notesNum)
        {
            // 内側を１本のスティックで叩いた時
            case (int)NOTES_TYPE.DO_NOTE:
                m_instancePos.y = this.transform.position.y;
                break;

            // 内側を２本のスティックで叩いた時
            case (int)NOTES_TYPE.DON_NOTE:
                m_instancePos.y = this.transform.position.y - subValue;
                break;

            // 外側を１本のスティックで叩いた時
            case (int)NOTES_TYPE.KA_NOTE:
                m_instancePos.y = this.transform.position.y - subValue * 2;
                break;

            // 外側を２本のスティックで叩いた時
            case (int)NOTES_TYPE.KAN_NOTE:
                m_instancePos.y = this.transform.position.y - subValue * 3;
                break;

            default:
                break;
        }
        return m_instancePos.y;
    }


    // ノーツが生成される度にカウントをする処理
    private void UPCountNotes()
    {
        if(m_countnNotes < MAX_NOTES)
        {
            m_countnNotes++;
        }
    }


    // カウントをリセットする
    public void ResetCount()
    {
        m_countnNotes = 0;
    }
}
