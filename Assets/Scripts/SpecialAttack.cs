using UnityEngine;

[RequireComponent(typeof(Health))]

///<summary>
/// This script is a parent class for the special attacks that the player or boss can use
/// </summary>
///<author>Jackson Codd</author>
///<version>1.0 Build 2024.11.29</version>
public class SpecialAttack : MonoBehaviour
{
    /// <summary>
    /// The name of the attack
    /// </summary>
    [SerializeField] string AttackName;

    /// <summary>
    /// The damage of the attack
    /// </summary>
    [SerializeField] float damage;

    /// <summary>
    /// The cooldown of the attack
    /// </summary>
    [SerializeField] float cooldown;

    /// <summary>
    /// The time that the attack will start
    /// </summary>
    [SerializeField] float startTime;

    /// <summary>
    /// The max health that the player can be at to attack
    /// </summary>
    [SerializeField] float maxHealthToAttack = float.MaxValue;

    /// <summary>
    /// The min health that the player can be at to attack
    /// </summary>
    [SerializeField] float minHealthToAttack = -1;

    /// <summary>
    /// start performing the attack on the timer
    /// </summary>
    
    private void OnEnable()
    {
        InvokeRepeating(nameof(Attack), startTime, cooldown);
    }

    /// <summary>
    /// Start the attack
    /// </summary>
    public void StartAttack()
    {
        if (GetComponent<Health>().GetHealth() <= maxHealthToAttack && GetComponent<Health>().GetHealth() > minHealthToAttack)
        {
            Attack();
        }
    }

    /// <summary>
    /// Perform the attack
    /// </summary>
    public virtual void Attack()
    {
        Debug.Log("Attacking");
    }

}
