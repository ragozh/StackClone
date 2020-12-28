using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] FlatController _flatPrefab;
    [SerializeField] FlatSO _flatSO;
    [SerializeField] StackGameManager _gameManager;

    public void Spawn()
    {
        var flat = Instantiate(_flatPrefab);
        flat.transform.position = new Vector3(
            transform.position.x,
            _flatSO.LastFlat.transform.position.y 
            + _flatSO.LastFlat.transform.localScale.y / 2 
            + _flatPrefab.transform.localScale.y / 2,
            transform.position.z
        );
        flat.OnGameOver += _gameManager.IsGameOver;
    }
}
