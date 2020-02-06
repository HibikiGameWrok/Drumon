public class WeakChecker_Script
{
    static public float WeakCheck(CreatureDataEntity.ELEM attacker, CreatureDataEntity.ELEM defender)
    {
        int checker = ((int)attacker + 1) % (int)CreatureDataEntity.ELEM.NUM - ((int)defender + 1) % (int)CreatureDataEntity.ELEM.NUM;

        if (checker < 0)
            return 1.5f;
        else if (checker > 0)
            return 0.5f;
        return 1.0f;
    }
}
