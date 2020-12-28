using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProjectile : MonoBehaviour
{    
    [SerializeField] float _flySpeed = 7;
    CharacterLaunchProjectile _launcher;
    CharacterController _launcherController;
    CharacterController _targetController;
    Vector3? _targetPosition;
    Vector3 _contactPoint;
    Rigidbody _rigidbody;
    Collider _collider;
    bool _hit = false;
    bool _seeker = false;
    float _maxLifeTime = 10f;
    float _lifeTime;
    AudioSource _audioSource;
    ParticleController _particleController;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    void OnEnable()
    {
        _flySpeed = 5;
        _audioSource = GetComponentInChildren<AudioSource>();
        _particleController = GetComponentInChildren<ParticleController>();
        _lifeTime = Time.time + _maxLifeTime;
    }

    void Update()
    {
        if (_hit)
        {
            OnHitTarget();
        }
        if (ShouldBackToPool())
        {
            ReturnToPool();
        }
    }

    bool ShouldBackToPool() => Time.time > _lifeTime;

    void ReturnToPool()
    {
        gameObject.SetActive(false);
        _launcher?.BackToPool(this);
    }

    void FixedUpdate()
    {
        Vector3 velocity = new Vector3();
        if (_targetPosition.HasValue)
            velocity = _targetPosition.Value - transform.position;
        else
            velocity = _targetController.transform.position - transform.position;
        _rigidbody.velocity = velocity * _flySpeed;
        if (_rigidbody.velocity == Vector3.zero)    return;
        _rigidbody.rotation = Quaternion.LookRotation(_rigidbody.velocity);
    }

    public void SetLauncher(CharacterLaunchProjectile launcher) => _launcher = launcher;

    public void SetDatas(CharacterController targetController, CharacterController launcherController)
    {
        _targetController = targetController;
        _launcherController = launcherController;
        if (_seeker)    _targetPosition = null;
        else    _targetPosition = _targetController.transform.position;
    }

    void OnCollisionEnter(Collision other)
    {
        if (!_targetController) return;
        if (_launcherController && other.gameObject.CompareTag(_launcherController.gameObject.tag))  return;
        if (other.gameObject.CompareTag(_targetController.gameObject.tag))
        {
            _targetController.HealthChange(-_launcherController.DealDamage());
        }
        _contactPoint = other.contacts[0].point;
        _hit = true;
    }

    void OnHitTarget()
    {
        _flySpeed = 0;
        AudioAndParticle();
        StartCoroutine(DisableProjectile());
        _hit = false;
    }

    IEnumerator DisableProjectile()
    {
        yield return new WaitForSeconds(0.1f);
        ReturnToPool();
    }

    void AudioAndParticle()
    {
        _particleController.SetEmitPoint(_contactPoint);
        _particleController.EmitParticle();
        _audioSource.Play();
    }
}
