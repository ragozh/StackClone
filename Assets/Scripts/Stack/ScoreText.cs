using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    [SerializeField] ScoreSO _score;
    Text _text;
    void Start() => _text = GetComponent<Text>();

    // Update is called once per frame
    void Update() => _text.text = _score.Score.ToString();
}
