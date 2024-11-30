using UnityEngine;

/// <summary>
/// This script is for the special attack that summons minions
/// </summary>
/// <author>Jackson Codd</author>
/// <version>1.0 Build 2024.11.29</version>
public class SummonAttack : SpecialAttack
{
    /// <summary>
    /// The type of minion to summon
    /// </summary>
    [SerializeField] GameObject summonType;

    /// <summary>
    /// The time offset of the summon
    /// </summary>
    [SerializeField] float summonOffset;

    /// <summary>
    /// The health of the minions
    /// </summary>
    [SerializeField] float summonHealth;

    /// <summary>
    /// The amount of minions to summon
    /// </summary>
    [SerializeField] float summonAmount;

    /// <summary>
    /// summon the minions for the attack see SpecialAttack.cs for more context
    /// </summary>
    public override void Attack()
    {
        for (int i = 0; i < summonAmount; i++)
        {
            GameObject summon = Instantiate(summonType, transform.position, Quaternion.identity);
            summon.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
            summon.transform.position += summon.transform.forward * summonOffset;
            summon.GetComponent<Health>().TakeDamage(summon.GetComponent<Health>().maxHealth - summonHealth);
        }
    }
}
