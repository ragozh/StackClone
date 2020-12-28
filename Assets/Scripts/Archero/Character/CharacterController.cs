using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterController : MonoBehaviour
{
    CharacterData _characterData;
    CharacterMovement _characterMovement;
    CharacterRotation _characterRotation;
    CharacterAttack _characterAttack;
    CharacterAnimationController _characterAnimCtrl;
    CharacterLaunchProjectile _characterLauncher;
    PlayerInput _playerInput;
    bool _isMoving;
    bool _isPlayer;
    
    public event Action<float> OnHealthChange = delegate { };
    void Awake()
    {
        _characterData = GetComponent<CharacterData>();
        _characterMovement = GetComponent<CharacterMovement>();
        _characterRotation = GetComponent<CharacterRotation>();
        _characterAttack = GetComponent<CharacterAttack>();
        _characterAnimCtrl = GetComponent<CharacterAnimationController>();
        _characterLauncher = GetComponent<CharacterLaunchProjectile>();
        _isPlayer = gameObject.CompareTag("Player");
        if (_isPlayer)
            _playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        _isMoving = false;
        _characterData.CurrentHP = _characterData.HP;
        HealthChange(0);
        _characterMovement.SetMoveSpeed(_characterData.Movespeed);
        _characterAnimCtrl.RescaleAttackAnimations((float) 1 / _characterData.AttackSpeed);

        if (!_isPlayer)
            _characterData.CurrentTarget = GameObject.Find("Player").GetComponent<CharacterController>();
    }
    
    void Update()
    {
        GetCurrentTarget();
        AttackController();
        if (_isPlayer)
            MoveController();
        
        if (_characterData.CurrentHP <= 0)  GameObject.Destroy(gameObject);
    }

    #region Movement
    void MoveController()
    {
        if (ShouldMove())
        {
            var targetPos = _playerInput._destination.Value;
            _characterRotation.SetTargetPosition(targetPos);
            //if (!_characterRotation.IsLookinTarget(targetPos)) return; // This slow down the movespeed
            Move(targetPos);
        }
        else if (_isMoving)
            MoveStop();
    }

    bool ShouldMove() => _playerInput._destination.HasValue;

    void Move(Vector3 targetPosition)
    {
        _characterMovement.SetTarget(targetPosition);
        _isMoving = true;
        _characterAnimCtrl.SetMovementAnimation(_isMoving);
    }

    void MoveStop()
    {
        _isMoving = false;
        _characterAnimCtrl.SetMovementAnimation(_isMoving);    
    }
    #endregion

    #region Attack
    void AttackController()
    {
        if (ShouldAttack())
        {
            if (!_characterData.CurrentTarget)   return;
            var targetPosition = _characterData.CurrentTarget.transform.position;
            _characterRotation.SetTargetPosition(targetPosition);
            if (!_characterRotation.IsLookinTarget(targetPosition)) return;
            _characterAttack.Attack((float) 1 / _characterData.AttackSpeed);
            _characterAnimCtrl.TriggerAttackAnimation("Character@Attacking1");
        }
    }

    bool ShouldAttack() => !_isMoving && _characterAttack.ShouldAttack();

    public void LaunchProjectile() => _characterLauncher.LauchProjectileFromLauncher(_characterData.CurrentTarget, this);
    
    public void HealthChange(float amount)
    {
        _characterData.CurrentHP += amount;

        var currentHealthPercent = (float) _characterData.CurrentHP / (float) _characterData.HP;
        OnHealthChange(currentHealthPercent);
    }

    public float DealDamage() => _characterData.AttackDamage;
    #endregion

    #region Detect Enemies
    void GetCurrentTarget()
    {
        if (_characterData.CurrentTarget || !CheckAvailableEnemies())  return;
        _characterData.CurrentTarget = GetClosestEnemy();
    }

    bool CheckAvailableEnemies()
    {
        var availableEnemies = false;
        if (_characterData.ListEnemies.Where(x => x != null).ToList().Count > 0)    availableEnemies = true;
        return availableEnemies;
    }

    CharacterController GetClosestEnemy()
    {
        var enemiesOrdered = _characterData.ListEnemies
                        .OrderBy(x => Mathf.Abs((x.transform.position - transform.position).magnitude))
                        .ToList();
        if (enemiesOrdered.Count > 0 && enemiesOrdered.First())
            return enemiesOrdered.First();
        return null;
    }

    public void SetListTargets(List<CharacterController> listEnemies)
    {
        _characterData.ListEnemies = listEnemies;
    }
    #endregion
}
