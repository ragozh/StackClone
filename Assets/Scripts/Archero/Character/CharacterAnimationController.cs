using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimationController : MonoBehaviour
{
    Animator _animator;
    List<AnimationClip> _listAnimationClips;
    List<AnimationClip> _listAttackAnimation;

    readonly int MovingAnimator = Animator.StringToHash("moving");    
    readonly int ScaleAsAnimator = Animator.StringToHash("scale_as");
    void Awake()
    {        
        _animator = GetComponent<Animator>();
        _listAnimationClips = _animator.runtimeAnimatorController.animationClips.ToList();
        _listAttackAnimation = _listAnimationClips.Where(x => x.name.StartsWith("Character@Attacking")).ToList();
    }

    public void SetMovementAnimation(bool isMoving)
    {
        if(_animator.GetBool(MovingAnimator) == isMoving)   return;
        _animator.SetBool(MovingAnimator, isMoving);
    }

    public void RescaleAttackAnimations(float attackDelay)
    {
        foreach (var attackAnimation in _listAttackAnimation)
        {
            if (!attackAnimation)   return;
            var attackAnimationLength = attackAnimation.length; // get the duration of animation         
            var _scaleAs = attackAnimationLength / attackDelay;  // rescale animation length
            _animator.SetFloat(ScaleAsAnimator, _scaleAs);
        }
    }

    public void TriggerAttackAnimation(string attackAnimationName)
    {
        _animator.SetTrigger(attackAnimationName);
    }
}
