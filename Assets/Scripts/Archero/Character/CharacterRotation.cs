using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    [SerializeField] float _rotateSpeed = 360;
    bool _shouldRotate;
    Vector3? _targetPosition;
    void OnEnable()
    {
        _shouldRotate = true;
        _targetPosition = null;
    }

    void Update()
    {
        if (!_shouldRotate) return; // Control rotation
        if (!_targetPosition.HasValue)   return;  // No target

        var offset = CheckRotation(_targetPosition.Value);
        if (IsLookinTarget(_targetPosition.Value)) // Stop turning if degree less than 5
        {
            _targetPosition = null;
            return;
        }
        Rotate(offset > 0 ? 1f : -1f);
    }

    void Rotate(float direction)
    {
        var turn = _rotateSpeed * direction * Time.deltaTime;
        transform.Rotate(0f, turn, 0f);
    }

    float CheckRotation(Vector3 targetPosition)
    {
        var fromVector = new Vector2(transform.forward.x, transform.forward.z);
        var toVector = new Vector2(
            targetPosition.x - transform.position.x,
            targetPosition.z - transform.position.z
        );
        var cross = Vector3.Cross(
            transform.forward,
            new Vector3(toVector.x, 0f, toVector.y)
        );
        var direction = cross.y > 0 ? 1 : -1;
        var result = Vector3.Angle(fromVector, toVector) * direction;
        return result;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    public void ShouldRotate(bool shouldRotate)
    {
        _shouldRotate = shouldRotate;
    }

    public bool IsLookinTarget(Vector3 targetPosition) => Mathf.Abs(CheckRotation(targetPosition)) < 5;
}
