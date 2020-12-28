using UnityEngine;
using UnityEngine.UI;

public class SpeedButton : MonoBehaviour
{
    int _speed;
    Text _text;
    void Awake()
    {
        _text = GetComponentInChildren<Text>();
        _speed = 1;
    }

    public void ChangeSpeed()
    {
        if (_speed >= 5)    _speed = 1;
        else    _speed++;
        _text.text = "X" + _speed;
        Time.timeScale = _speed;
    }
}
