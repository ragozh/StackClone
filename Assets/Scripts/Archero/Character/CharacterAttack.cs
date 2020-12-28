using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    float _nextAttackTime;
    float _attackDelay;

    public bool ShouldAttack() => Time.time > _nextAttackTime;

    public void Attack(float attackDelay)
    {
        _attackDelay = attackDelay;
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        if (!ShouldAttack()) yield break;

        _nextAttackTime = Time.time + _attackDelay;

        yield return StartCoroutine(OnAttackStart());
        yield return StartCoroutine(OnAttackContinue());
        yield return StartCoroutine(OnAttackEnd());
    }

    IEnumerator OnAttackStart()
    {
        yield return null;
    }

    IEnumerator OnAttackContinue()
    {
        yield return StartCoroutine(DoAttack());
    }
    IEnumerator OnAttackEnd()
    {
        yield return null;
    }

    IEnumerator DoAttack()
    {
        yield return new WaitForSeconds(_attackDelay);
    }
}
