using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLaunchProjectile : MonoBehaviour
{
    [SerializeField] private CharacterProjectile _projectilePrefab;
    [SerializeField] private GameObject _shootPoint;
    Queue<CharacterProjectile> _pool = new Queue<CharacterProjectile>();

    public void LauchProjectileFromLauncher(CharacterController currentTarget, CharacterController launcher)
        {
            var projectile = GetProjectile();
            projectile.SetDatas(currentTarget, launcher);
        }

    #region objects pooling
    CharacterProjectile GetProjectile()
    {
        if (_pool.Count == 0)
        {
            AddNewProjectile();
        }
        var projectile = _pool.Dequeue();
        projectile.transform.position = _shootPoint.transform.position;
        projectile.transform.rotation = _shootPoint.transform.rotation;
        projectile.gameObject.SetActive(true);
        return projectile;
    }

    void AddNewProjectile()
    {
        var newProjectile = Instantiate(_projectilePrefab,
                                            _shootPoint.transform.position, _shootPoint.transform.rotation);
        newProjectile.SetLauncher(this);
        newProjectile.gameObject.SetActive(false);
        BackToPool(newProjectile);
    }

    public void BackToPool(CharacterProjectile projectile)
    {
        _pool.Enqueue(projectile);
    }
    #endregion
}
