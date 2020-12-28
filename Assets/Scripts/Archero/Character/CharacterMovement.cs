using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterMovement : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;
    float _moveSpeed = 4;
    void Awake() => _navMeshAgent = GetComponent<NavMeshAgent>();

    public void SetTarget(Vector3 targetPosition) => _navMeshAgent.Move((targetPosition - transform.position).normalized * Time.deltaTime * _moveSpeed);

    public void ClearTarget() => _navMeshAgent.ResetPath();

    public void SetMoveSpeed(float moveSpeed) => _moveSpeed = moveSpeed;
}
