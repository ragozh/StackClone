using UnityEngine;

public class ParticleSystemAutoDestroy : MonoBehaviour
{
    ParticleSystem _particleSystem;
    private void Start() => _particleSystem = GetComponent<ParticleSystem>();

    private void Update()
    {
        if (!_particleSystem) return;
        if (!_particleSystem.IsAlive())
            Destroy(gameObject);
    }
}
