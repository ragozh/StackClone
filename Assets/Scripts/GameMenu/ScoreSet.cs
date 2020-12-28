using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSet : MonoBehaviour
{
    [SerializeField] ScoreSO StackScore;
    [SerializeField] ScoreSO BallScore;
    void Awake()
    {
        StackScore.HighScore = PlayerPrefs.GetInt("Stack");
        BallScore.HighScore = PlayerPrefs.GetInt("Ball");
    }
}
