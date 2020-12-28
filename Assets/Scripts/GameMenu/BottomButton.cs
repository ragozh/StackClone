using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomButton : MonoBehaviour
{
    [SerializeField] GameObject _panel;
    GameObject[] _listPanel;
    void Awake() => _listPanel = GameObject.FindGameObjectsWithTag("UIPanel");

    public void SwitchPanel()
    {
        var direction = Vector3.zero - _panel.transform.localPosition;
        for(int i = 0; i < _listPanel.Length; i++)
        {
            _listPanel[i].transform.position += direction;
        }
    }
}
