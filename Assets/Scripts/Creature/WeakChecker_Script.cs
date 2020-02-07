public class WeakChecker_Script
{
    static public float WeakCheck(CreatureDataEntity.ELEM attacker, CreatureDataEntity.ELEM defender)
    {
        int checker = ((int)attacker + 1) % (int)CreatureDataEntity.ELEM.NUM - ((int)defender + 1) % (int)CreatureDataEntity.ELEM.NUM;

        if (checker.Equals(-1))
            return 1.5f;
        else if (checker.Equals(1))
            return 0.5f;
        return 1.0f;
    }
}
