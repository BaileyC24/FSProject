using System.Collections;
using UnityEngine;

public class healing : MonoBehaviour
{
    enum HealingType
    {
        Temporary,
        Permanent,
        TimedReusable,
        Regen
    }

    [Header("Type")]
    [SerializeField] HealingType type;

    [Header("Healing")]
    [SerializeField] int healingAmount;
    [SerializeField] float cooldown;

    [Header("Regen")]
    [SerializeField] float healingRate;

    [Header("Limited Uses")]
    [SerializeField] int uses;

    bool isHealing;
    bool onCooldown;
    int originalUses;

    void Start()
    {
        originalUses = uses;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        IHealable healable = other.GetComponent<IHealable>();
        if (healable == null || type == HealingType.Regen || onCooldown)
            return;

        if (other.CompareTag("Player"))
        {
            healable.heal(healingAmount);
            StartCoroutine(HandleUse());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;

        if (type != HealingType.Regen || isHealing)
            return;

        IHealable healable = other.GetComponent<IHealable>();
        if (healable != null)
        {
            StartCoroutine(Regen(healable));
        }
    }

    IEnumerator HandleUse()
    {
        onCooldown = true;

        if (type == HealingType.Temporary || type == HealingType.TimedReusable)
            uses--;

        if (type == HealingType.Temporary && uses <= 0)
        {
            Destroy(gameObject);
            yield break;
        }

        if (type == HealingType.TimedReusable && uses <= 0)
        {
            yield return new WaitForSeconds(cooldown);
            uses = originalUses;
        }

        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }

    IEnumerator Regen(IHealable healable)
    {
        isHealing = true;
        healable.heal(healingAmount);
        yield return new WaitForSeconds(healingRate);
        isHealing = false;
    }
}
