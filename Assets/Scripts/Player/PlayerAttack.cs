using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform AttackPos;
    [Range(0.1f, 2)][SerializeField] public float attackSpeed;
    [Range(1, 15)][SerializeField] public int str;
    [SerializeField] private Color damageColor;
    private float attackTimer;
    private bool attacking;
    private readonly int AttackName = Animator.StringToHash("Attack");
    private AttackTrigger attackTrigger;
    private PlayerStateMachine PSM;

    private void Start()
    {
        attackTrigger = GetComponentInChildren<AttackTrigger>();
        attackTrigger.ATKTRIGGER += CheckAttack;
        PSM = GetComponent<PlayerStateMachine>();
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;

        if (gameManager.instance.playerScript.GetInput().Player.Attack.triggered
            && attackTimer >= attackSpeed && !attacking)
        {
            Attack();
        }
    }

    void Attack()
    {
        StartCoroutine(AttackWindow());
        AttackAudio();
    }

    void CheckAttack(Collider _target)
    {
        if (!attacking || !_target.CompareTag("Enemy")) return;
        
        IDamage damage = _target.GetComponent<IDamage>();
        if (damage != null)
            damage.takeDamage(str);
    }

    void AttackAudio() {
        StartCoroutine(DelayedAttackAudio());
    }

    IEnumerator DelayedAttackAudio() {
        yield return new WaitForSeconds(0.75f); 
        if (PSM != null && PSM.aud != null && PSM.audHit.Length > 0 && PSM.audHit[0] != null)
            PSM.aud.PlayOneShot(PSM.audHit[0], PSM.volume);
        else
            Debug.LogError("Attack Audio failed — missing PSM, aud, or audio clip");
    }
    
    IEnumerator AttackWindow()
    {
        attacking = true;
        attackTimer = 0f;
        gameManager.instance.playerScript.GetAnimator().SetTrigger(AttackName);
        yield return new WaitForSeconds(gameManager.instance.playerScript.GetAnimator().GetCurrentAnimatorClipInfo(0).Length);
        attacking = false;
    }
}