using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] private GameObject[] _particles;

    [SerializeField] private Dictionary<string, Transform> Diction;
    [SerializeField] private Transform _emitPoint;
    private Vector3 _emitPosition;

    public void EmitParticle(int particleIndex = 0)
    {
        if (_emitPosition == null && _emitPoint)  // priority position first
            _emitPosition = _emitPoint.position;
        Instantiate(_particles[particleIndex], _emitPosition, Quaternion.identity);
    }

    public void SetEmitPoint(Vector3 emitPosition)
    {
        _emitPosition = emitPosition;
    }
}
