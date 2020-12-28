using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Score")]
public class ScoreSO : ScriptableObject
{
    public int Score;
    public int HighScore;
    void OnEnable()
    {
        Score = 0;
    }
}
