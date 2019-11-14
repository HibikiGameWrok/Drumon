/*----------------------------------------------------------*/
//  file:      GameStateManager_Script.cs                      |
//				 											                    |
//  brief:    ゲームの状態の管理スクリプト				        |
//                                      				                        |
//															                    |
//  date:	2019.11.12									            |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

// ゲームステートマネージャークラスの定義
public class GameStateManager : MonoBehaviour
{
    // ゲームステート
    private ReactiveProperty<GameState> m_gameState = new ReactiveProperty<GameState>(GameState.Initialize);
    // 現在のゲームステート
    public IReadOnlyReactiveProperty<GameState> CurrentState => m_gameState;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void MoveToTitleScene()
    {
        SceneManager.LoadScene("Title");
    }
}
