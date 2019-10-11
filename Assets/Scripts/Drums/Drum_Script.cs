﻿/*----------------------------------------------------------*/
//  file:      Drum_Script.cs					                        |
//				 															    |
//  brief:    ドラムの基底クラスのスクリプト				    |
//              Drum base class 				                        |
//																				|
//  date:	2019.10.9												    |
//																				|
//  author: Renya Fukuyama										|
/*----------------------------------------------------------*/

// using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Drumクラスの定義
public class Drum_Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// 当たり判定の検出をする
    /// </summary>
    public void OnCollisionEnter(Collision col)
    {
        Debug.Log(this.tag);
        if (this.tag == "Drum_Edge")
        {
            Debug.Log("edge_hit");
        }
        if (this.tag == "Drum_Center")
        {
            Debug.Log("center_hit");
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        //Debug.Log(this.tag);
        if (this.tag == "Drum_Edge")
        {
            Debug.Log("edge_hit");
        }
        if (this.tag == "Drum_Center")
        {
            Debug.Log("center_hit");
        }
    }
}
