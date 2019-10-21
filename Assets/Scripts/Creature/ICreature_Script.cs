public interface ICreature_Script
{
    void Execute();
    void CountTimer();
    void Attack();
    void Damage(int damage);
    void Heal();
    CharactorData GetData();
    void SetTarget(ICreature_Script target);
    void Dead();
}
