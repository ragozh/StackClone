using UnityEngine;

public class LaunchPreview : MonoBehaviour
{
    LineRenderer _lineRenderer;
    Vector3 _startPoint;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }
    
    public void SetStartPoint(Vector3 startPoint)
    {
        _startPoint = startPoint;
        _lineRenderer.SetPosition(0, startPoint);
    }

    public void SetEndPoint(Vector3 endPoint)
    {
        var line = endPoint - _startPoint;
        if (line.magnitude > 2.5f)
            line = line.normalized * 2.5f;
        _lineRenderer.SetPosition(1, _startPoint + line);
    }

    public void Reset()
    {
        _lineRenderer.SetPosition(0, Vector3.zero);
        _lineRenderer.SetPosition(1, Vector3.zero);
    }
}
