using UnityEngine;

public class ScaleController_Script : MonoBehaviour
{
    private float m_timer;
    private float m_endTime = 0.0f;

    public float EndTime
    {
        set { m_endTime = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        float size = Mathf.Lerp(1.0f, 0.0f, m_timer / m_endTime);
        this.transform.localScale = Vector3.one * size;
    }
}
