using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class FlatController : MonoBehaviour
{
    [SerializeField] FlatSO _flatSO;
    [SerializeField] float _slideSpeed;
    Transform _lastFlat;
    Color _newColor;
    public event Action OnGameOver = delegate { };
    void OnEnable()
    {        
        gameObject.name = "Flat " + _flatSO.Count++;
        if (!_flatSO.LastFlat)  _flatSO.LastFlat = GameObject.Find("Flat 0").GetComponent<FlatController>();
        _flatSO.CurrentFlat = this;
        _lastFlat = _flatSO.LastFlat.gameObject.transform;
        
        _newColor = RandomColor();
        GetComponent<Renderer>().material.SetColor("_Color", _newColor);

        if (_lastFlat.name == "Flat 0") return;
        transform.localScale = _lastFlat.transform.localScale;

        SetSlideSpeed();
        if (((_flatSO.Count - 2) % 20) == 0)  Camera.main.transform.position += Vector3.up * 1.5f;
    }

    internal void Pause() => _slideSpeed = 0;
    internal void SetSlideSpeed() => _slideSpeed = (_flatSO.Count - 2) / 15 + 1;

    Color RandomColor()
    {
        return new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
    }

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * _slideSpeed;
        if (transform.position.z > 5)   OnGameOver();
    }

    internal bool Stop()
    {
        _slideSpeed = 0;
        if (!IsValidStack())
        {
            OnGameOver();
            gameObject.AddComponent<Rigidbody>();
            return false;
        }
        var offset = transform.position.z - _lastFlat.position.z; // Get offset
        if (IsPerfect(offset))
        {   
            Perfect();
            _flatSO.IsPerfect = true;
        }
        else
        {
            Chop(offset);
            _flatSO.IsPerfect = false;
        }
        _flatSO.LastFlat = this;
        return true;
    }

    bool IsValidStack()
    {
        var distance = Mathf.Abs(transform.position.z - _lastFlat.position.z);
        if (distance >= _lastFlat.localScale.z) return false;
        return true;
    }
    bool IsPerfect(float offset) => Mathf.Abs(offset) <= 0.1;
    void Perfect()
    {
        transform.position = new Vector3(
            _lastFlat.position.x, 
            _lastFlat.position.y + _lastFlat.localScale.y / 2 + transform.localScale.y / 2,
            _lastFlat.position.z
        );
    }
    void Chop(float offset)
    {
        var direction = offset > 0 ? 1 : -1; // Get direction
        // Calculate sizes
        var newSizeZ = _lastFlat.localScale.z - Mathf.Abs(offset);
        var splitedSizeZ = transform.localScale.z - newSizeZ;
        // Calculate position
        var newPosZ = _lastFlat.position.z + (offset / 2);
        // Change current flat
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newSizeZ);
        transform.position = new Vector3(transform.position.x, transform.position.y, newPosZ);
                
        var edge = transform.position.z + (newSizeZ / 2 * direction);
        var splitedPosZ = edge + (splitedSizeZ / 2 * direction);

        CreateSplitedFlat(splitedSizeZ, splitedPosZ);
    }

    void CreateSplitedFlat(float splitedSizeZ, float splitedPosZ)
    {
        var newFlat = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newFlat.gameObject.name = gameObject.name + ".1";
        newFlat.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, splitedSizeZ);
        newFlat.transform.position = new Vector3(transform.position.x, transform.position.y, splitedPosZ);
        newFlat.GetComponent<Renderer>().material.SetColor("_Color", _newColor);

        newFlat.AddComponent<Rigidbody>();
        GameObject.Destroy(newFlat, 1.5f);
    }
}
