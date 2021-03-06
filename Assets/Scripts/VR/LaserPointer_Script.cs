﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class LaserPointer_Script : MonoBehaviour
{
    [SerializeField]
    private Transform m_handAnchor = null;
    [SerializeField]
    private LineRenderer m_lineRenderer = null;
    [SerializeField]
    private float m_maxRayDistance = 500f;


    void Awake()
    {
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                Ray laserPointer = new Ray(m_handAnchor.position, m_handAnchor.forward);

                // 作成したRay上にColliderがあるか判定
                RaycastHit hit;
                if (Physics.Raycast(laserPointer, out hit, m_maxRayDistance))
                {
                    // Colliderがあれば、衝突箇所までレーザーを描画する
                    RenderLaserToHit(laserPointer, hit);
                }
                else
                {
                    // Colliderがなければ、最大長のレーザーを描画する
                    RenderLaserFullLength(laserPointer);
                }
            })
            .AddTo(gameObject);
    }


    private void RenderLaserToHit(Ray ray, RaycastHit hit)
    {
        RenderLaser(ray.origin, hit.point);
    }

    private void RenderLaserFullLength(Ray ray)
    {
        RenderLaser(ray.origin, ray.origin + ray.direction * m_maxRayDistance);
    }

    private void RenderLaser(Vector3 from, Vector3 to)
    {
        m_lineRenderer.SetPosition(0, from);
        m_lineRenderer.SetPosition(1, to);
    }
}
