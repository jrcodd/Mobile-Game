using Riptide;
using UnityEngine;

public enum EnemyType
{
    Grunt = 0,
    Lich,
    Golem,
}
[RequireComponent(typeof(Health))]
public class EnemyControllerMultiplayer : MonoBehaviour
{

    [SerializeField] private AttackIndicatorController attackIndicator;
    [SerializeField] private EnemyType enemyType;
    private Health healthObject;
    private Vector3 position;
    private Vector3 forward;
    private int attackType;

    public Arena Arena { get; set; }


    public ushort Id { get; set; }

    private void OnEnable()
    {
        healthObject = GetComponent<Health>();
    }

    public Health GetHealthComponent()
    {
        return healthObject;
    }
    public void SetIndicatorFillAmount(float amount)
    {
        if(attackIndicator != null)
        attackIndicator.SetFillAmount(amount);
    }
    public int GetAttackType()
    {
        return attackType;
    }
    public void SetAttackType(int type)
    {
        attackType = type;
    }

    public void SetInputs(Vector3 _position, Vector3 _forward)
    {
        position = _position;
        forward = _forward;
    }
    private void FixedUpdate()
    {
        transform.position = position;
        transform.forward = forward;
    }

}
