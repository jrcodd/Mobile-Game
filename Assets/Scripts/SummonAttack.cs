using UnityEngine;

public class SummonAttack : SpecialAttack
{
    [SerializeField] GameObject summonType;
    [SerializeField] float summonOffset;
    [SerializeField] float summonHealth;
    [SerializeField] float summonAmount;
    public override void Attack()
    {
        print("summoning help");
        for (int i = 0; i < summonAmount; i++)
        {
            GameObject summon = Instantiate(summonType, transform.position, Quaternion.identity);
            summon.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
            summon.transform.position += summon.transform.forward * summonOffset;
            summon.GetComponent<Health>().TakeDamage(summon.GetComponent<Health>().maxHealth - summonHealth);
        }
    }
}
