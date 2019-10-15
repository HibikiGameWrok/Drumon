using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesGage : MonoBehaviour
{
    Slider m_notesSlider;

    // Start is called before the first frame update
    void Start()
    {
        m_notesSlider = GetComponent<Slider>();

        float maxPoint = 200.0f;
        float minPoint = 0.0f;
        float nowPoint = 0.0f;

        m_notesSlider.maxValue = maxPoint;
        m_notesSlider.minValue = minPoint;
        m_notesSlider.value = nowPoint;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
