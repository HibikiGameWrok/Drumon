using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SO_ElementChecker", menuName = "ElementChecker Data")]
public class SO_ElementChecker : ScriptableObject
{
    public enum ELEM
    {
        FIRE,
        WIND,
        VOID,
        EARTH,
        WATER
    };

    [SerializeField]
    private float m_multiplierSuperEffective;
    [SerializeField]
    private float m_multiplierNotEffective;

    public float CompareElement(int _defensive, int _offensive)
    {
        ELEM defensive = (ELEM)_defensive;
        ELEM offensive = (ELEM)_offensive;
        
        switch(offensive)
        {
            case ELEM.FIRE:
                {
                    if (defensive == ELEM.WIND) return m_multiplierSuperEffective;
                    else if (defensive == ELEM.WATER) return m_multiplierNotEffective;
                }
                break;
            case ELEM.WIND:
                {
                    if (defensive == ELEM.VOID) return m_multiplierSuperEffective;
                    else if (defensive == ELEM.FIRE) return m_multiplierNotEffective;
                }
                break;
            case ELEM.VOID:
                {
                    if (defensive == ELEM.EARTH) return m_multiplierSuperEffective;
                    else if (defensive == ELEM.WIND) return m_multiplierNotEffective;
                }
                break;
            case ELEM.EARTH:
                {
                    if (defensive == ELEM.WATER) return m_multiplierSuperEffective;
                    else if (defensive == ELEM.VOID) return m_multiplierNotEffective;
                }
                break;
            case ELEM.WATER:
                {
                    if (defensive == ELEM.FIRE) return m_multiplierSuperEffective;
                    else if (defensive == ELEM.EARTH) return m_multiplierNotEffective;
                }
                break;
            default:
                break;
        }


        return 1;
    }

}
