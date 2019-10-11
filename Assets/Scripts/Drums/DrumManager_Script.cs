/*----------------------------------------------------------*/
//  file:      DrumManger_Script.cs		                        |
//				 															    |
//  brief:    ドラムマネージャーのスクリプト				    |
//              Drums 	Manager Class		                        |
//																				|
//  date:	2019.10.11											    |
//																				|
//  author: Renya Fukuyama										|
/*----------------------------------------------------------*/

// using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ドラムマネージャーの定義
public class DrumManager_Script : MonoBehaviour 
{
    // メンバ変数 

    // 攻撃用のドラム 
    [SerializeField]
    private AttackDrum_Script m_attackDrum;
    // 回復用のドラム
    [SerializeField]
    private HeelDrum_Script m_heelDrum;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
