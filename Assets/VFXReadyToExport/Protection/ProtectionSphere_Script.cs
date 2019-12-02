using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionSphere_Script : MonoBehaviour
{

    private Renderer rend;

    private float OriginalScaleValue = 4;
    public bool IsEnding = false;

    private int _MaxHitAmount = 20;
    private List<Vector4> _HitData = new List<Vector4>();

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        OriginalScaleValue = transform.localScale.x;

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsEnding)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(OriginalScaleValue, OriginalScaleValue, OriginalScaleValue), OriginalScaleValue * Time.deltaTime);
        }
        else
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0, 0, 0), OriginalScaleValue * Time.deltaTime);
            if (transform.localScale.magnitude <= 0.01) Destroy(gameObject);
        }

        for (int i = 0; i < _HitData.Count; ++i)
        {
            var the = _HitData[i]; 
            the.w += 10 * Time.deltaTime;
            _HitData[i] = the;

            if (_HitData[i].w >= 2)
            {
                //Delete
                _HitData.RemoveAt(i);
            }

        }

        Vector4[] hitdatatosend = new Vector4[_MaxHitAmount];
        for (int i = 0; i < _MaxHitAmount; ++i)
        {
            if (i < _HitData.Count)
            {
                hitdatatosend[i] = _HitData[i];
            }
            else
            {
                hitdatatosend[i] = new Vector4();
            }
        }

        rend.material.SetVectorArray("_HitData", hitdatatosend);
        rend.material.SetInt("_HitAmount", _HitData.Count);

    }

    void OnTriggerEnter(Collider col)
    {
        if (_HitData.Count < _MaxHitAmount)
        {
            Vector3 normalcalculate = Vector3.Normalize(col.gameObject.transform.position - gameObject.transform.position);
            _HitData.Add(new Vector4(normalcalculate.x, normalcalculate.y, normalcalculate.z, 0));
        }
    }

}
