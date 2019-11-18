using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmearShader_Script : MonoBehaviour
{
    [SerializeField]
    private Renderer theRenderer = null;

    Queue<Vector3> _recentPositions = new Queue<Vector3>();

    [SerializeField]
    int _frameLag = 0;

    void Start()
    {
        if (theRenderer == null) theRenderer = GetComponent<Renderer>();
    }

    void LateUpdate()
    {
        if (_recentPositions.Count > _frameLag)
            theRenderer.material.SetVector("_PrevPosition", _recentPositions.Dequeue());

        theRenderer.material.SetVector("_Position", transform.position);
        _recentPositions.Enqueue(transform.position);
    }
}
