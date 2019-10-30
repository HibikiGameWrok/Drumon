using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNotesInstance : MonoBehaviour
{
    // プレハブの名前を定数化
    private const string NOTES_NAME_PATH = "InsPrefab/NotePrefab";

    // ノーツ生成の最大値
    private const int MAX_NOTES = 8;

    private enum NOTES_TYPE : int
    {
        NONE = 0,
        DO_NOTE,
        DON_NOTE,
        KA_NOTE,
        KAN_NOTE,
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

        // 全４種分のノーツを４つ生成
        for (int i = 1; i < 4 + 1; i++)
        {
            InstanceNotes(i);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    
    // ノーツプレハブを番号によって生成する関数
    public void InstanceNotes(int num)
    {
        m_notesPrefab = Instantiate(
            SetLodeNotesPrefab(num),
            new Vector3(m_instancePos.x, SetInsPositionY(num), m_instancePos.z),
            Quaternion.identity) as GameObject;

        m_notesPrefab.transform.parent = this.transform;
    }

    
    // 生成するノーツプレハブの番号を設定し返す関数
    private GameObject SetLodeNotesPrefab(int num)
    {
        // ResourcesファイルからPrefabデータを設定(末尾の番号によってデータが変わる)
        m_notesPrefab = (GameObject)Resources.Load(NOTES_NAME_PATH + num);
        return m_notesPrefab;
    }


    // ノーツの種類によって生成座標のY座標を設定
    private float SetInsPositionY(int num)
    {
        float subValue = 0.1f;
        switch(num)
        {
            case (int)NOTES_TYPE.DO_NOTE:
                m_instancePos.y = this.transform.position.y;
                break;

            case (int)NOTES_TYPE.DON_NOTE:
                m_instancePos.y = this.transform.position.y - subValue;
                break;

            case (int)NOTES_TYPE.KA_NOTE:
                m_instancePos.y = this.transform.position.y - subValue * 2;
                break;

            case (int)NOTES_TYPE.KAN_NOTE:
                m_instancePos.y = this.transform.position.y - subValue * 3;
                break;

            default:
                break;
        }
        return m_instancePos.y;
    }

}
