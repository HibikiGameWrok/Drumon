using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class RotatingSun : MonoBehaviour
{
    [SerializeField]
    private float m_rotateSpeed = 0.1f;
    [SerializeField]
    private Vector3 m_rot = new Vector3(270f, 330f, 0f);


    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation = Quaternion.Euler(m_rot);
        // 現在のパソコンの時刻で太陽を傾ける
        var rotX = transform.localEulerAngles.x - 15f * DateTime.Now.Hour;
        // マイナスの値だったら補正する
        if (rotX < 0)
            rotX += 360f;

        // 時刻に合わせて角度を設定する
        transform.localEulerAngles = new Vector3(rotX, transform.localEulerAngles.y, transform.localEulerAngles.z);

        Observable.EveryLateUpdate()
            .Subscribe(_ =>
            {
                //徐々に回転させる
                transform.Rotate(-Vector3.right * m_rotateSpeed * Time.deltaTime);
            })
            .AddTo(this);
    }
}
