using UnityEngine;
using UnityEngine.UI;

public class LeftText : MonoBehaviour
{
    [SerializeField] BallSO _data;
    Text _text;
    void Start() => _text = GetComponent<Text>();

    // Update is called once per frame
    void Update() => _text.text = "  Ball count: " + _data.BallAmount.ToString(); // exxtra space for spacing
}
