using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    X_PLUS,
    X_MINUS,
    Z_PLUS,
    Z_MINUS
}

public class VFXControl_Peck_Script : MonoBehaviour
{

    public Direction m_direction;
    [Range(5, 20)]
    public float m_distance;
    [Range(0.01f, 5)]
    public float m_TimeInterval0;
    [Range(0.01f, 5)]
    public float m_TimeInterval1;
    [Range(0.01f, 5)]
    public float m_TimeInterval2;

    [SerializeField]
    private GameObject AttackStreaks_Prefab;
    [SerializeField]
    private GameObject WindTunnel_Prefab;
    [SerializeField]
    private GameObject HitEffect0_Prefab;
    [SerializeField]
    private GameObject HitEffect1_Prefab;

    private Transform ObjectPosition;
    private float MaxTime;
    private float CurrentTime;
    private Vector3 Origin;
    private Vector3 Destination;

    private float SpecificTimeToCreateHitEffect;

    // Start is called before the first frame update
    void Start()
    {
        ObjectPosition = transform.parent;
        MaxTime = m_TimeInterval0 + m_TimeInterval1 + m_TimeInterval2;
        CurrentTime = MaxTime;
        Origin = ObjectPosition.position;

        transform.eulerAngles = ObjectPosition.eulerAngles;

        if (m_direction == Direction.X_PLUS)
        {
            Destination = Origin + m_distance * new Vector3(1, 0, 0);
        }
        else if (m_direction == Direction.X_MINUS)
        {
            Destination = Origin + m_distance * new Vector3(-1, 0, 0);
        }
        else if (m_direction == Direction.Z_PLUS)
        {
            Destination = Origin + m_distance * new Vector3(0, 0, 1);
        }
        else if (m_direction == Direction.Z_MINUS)
        {
            Destination = Origin + m_distance * new Vector3(0, 0, -1);
        }

        GameObject temp_AttackStreaks = Instantiate(AttackStreaks_Prefab, transform);
        Destroy(temp_AttackStreaks, m_TimeInterval0);

        GameObject temp_WindTunnel = Instantiate(WindTunnel_Prefab);
        temp_WindTunnel.transform.position = transform.position;
        temp_WindTunnel = Instantiate(WindTunnel_Prefab);
        temp_WindTunnel.transform.position = transform.position;
        temp_WindTunnel.transform.Rotate(temp_WindTunnel.transform.forward, 111);
        temp_WindTunnel.transform.localScale = new Vector3(temp_WindTunnel.transform.localScale.x * 1.2f, temp_WindTunnel.transform.localScale.y, temp_WindTunnel.transform.localScale.z * 1.2f);
        //Destroy(temp_WindTunnel, CurrentTime);

        SpecificTimeToCreateHitEffect = ((5 / m_distance) * m_TimeInterval0) + m_TimeInterval1 + m_TimeInterval2;

    }

    // Update is called once per frame
    void Update()
    {
        CurrentTime -= Time.deltaTime;
        
        if (CurrentTime > MaxTime - m_TimeInterval0)
        {

            ObjectPosition.position = Vector3.MoveTowards(ObjectPosition.position, Destination, Time.deltaTime * m_distance / m_TimeInterval0);

            if (CurrentTime <= SpecificTimeToCreateHitEffect && CurrentTime + Time.deltaTime >= SpecificTimeToCreateHitEffect)
            {
                GameObject temp_HitEffect0 = Instantiate(HitEffect0_Prefab);
                temp_HitEffect0.transform.eulerAngles = ObjectPosition.eulerAngles;
                temp_HitEffect0.transform.Rotate(0, 0, -90);
                temp_HitEffect0.transform.position = Destination - 4 * (Destination - Origin).normalized;

                GameObject temp_HitEffect1 = Instantiate(HitEffect1_Prefab);
                temp_HitEffect1.transform.eulerAngles = ObjectPosition.eulerAngles;
                temp_HitEffect1.transform.Rotate(0, 0, -90);
                temp_HitEffect1.transform.position = Destination - 5 * (Destination - Origin).normalized;

                //Destroy(temp_HitEffect0, CurrentTime);
            }

        }
        else if (CurrentTime > MaxTime - m_TimeInterval0 - m_TimeInterval1)
        {

        }
        else if (CurrentTime > MaxTime - m_TimeInterval0 - m_TimeInterval1 - m_TimeInterval2)
        {

            ObjectPosition.position = Vector3.MoveTowards(ObjectPosition.position, Origin, Time.deltaTime * m_distance / m_TimeInterval2);

        }
        else
        {
            ObjectPosition.position = Origin;
            Destroy(gameObject);
        }

    }
}
