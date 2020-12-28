using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    #region Stats
    public float AttackRange;
    public float AttackDamage;
    public float AttackSpeed;
    public float HP;
    public float CurrentHP;
    public float Movespeed;
    #endregion

    #region Controller
    const float DETECT_DELAY = 5;
    public List<CharacterController> ListEnemies;
    public CharacterController CurrentTarget;
    #endregion

    void Awake()
    {
        if (ListEnemies == null)
            ListEnemies = new List<CharacterController>();
    }
}
