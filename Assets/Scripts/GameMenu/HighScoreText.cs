using UnityEngine;
using UnityEngine.UI;

public class HighScoreText : MonoBehaviour
{
    [SerializeField] ScoreSO _score;
    Text _text;
    [SerializeField] string _preText;

    void Start() => _text = GetComponent<Text>();

    // Update is called once per frame
    void Update() => _text.text = _preText + _score.HighScore; // exxtra space for spacing
}
