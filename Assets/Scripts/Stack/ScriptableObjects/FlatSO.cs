using UnityEngine;

[CreateAssetMenu(menuName = "Stack/Flat")]
public class FlatSO : ScriptableObject
{
    public bool IsPerfect;
    public int Count;
    public FlatController CurrentFlat;
    public FlatController LastFlat;
    void OnEnable() => Count = 0;
}
