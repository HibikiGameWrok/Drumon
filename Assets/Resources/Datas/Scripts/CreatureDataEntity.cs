[System.Serializable]
public class CreatureDataEntity
{
    public enum ELEM
    {
        FIRE,
        WIND,
        VOID,
        EARTH,
        WATER,
        NUM
    };

    public string name = "";

    public string hp = "";

    public string atk = "";

    public string def = "";

    public float waitTime = 0.0f;

    public ELEM elem = 0;

    public int basePoint = 0;
}
