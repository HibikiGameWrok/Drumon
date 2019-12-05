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

    public string name = null;

    public int maxHp = 0;

    public int hp = 0;

    public int atk = 0;

    public int def = 0;

    public float waitTime = 0.0f;

    public ELEM elem = 0;
}
