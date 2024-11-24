using UnityEngine;

[RequireComponent(typeof(Health))]

public class SpecialAttack : MonoBehaviour
{
    [SerializeField] string AttackName;
    [SerializeField] float damage;
    [SerializeField] float cooldown;
    [SerializeField] float startTime;
    [SerializeField] float maxHealthToAttack = float.MaxValue;
    [SerializeField] float minHealthToAttack = -1;


    private void OnEnable()
    {
        InvokeRepeating(nameof(Attack), startTime, cooldown);
    }
    public void StartAttack()
    {
        if (GetComponent<Health>().GetHealth() <= maxHealthToAttack && GetComponent<Health>().GetHealth() > minHealthToAttack)
        {
            Attack();
        }
    }
    public virtual void Attack()
    {
        Debug.Log("Attacking");
    }

}
