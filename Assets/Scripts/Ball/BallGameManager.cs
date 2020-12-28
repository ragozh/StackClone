using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BallGameManager : MonoBehaviour
{
    [SerializeField] Block _blockPrefab;
    [SerializeField] Block _bonusPrefab;
    [SerializeField] Text _midScreenText;
    [SerializeField] ScoreSO _score;
    [SerializeField] BallSO _data;
    const int COLUMN_COUNT = 5;
    int _rowCount = 1;
    float _blockDistance = 1;
    List<Block> _lstBLock;
    bool _isGamveOver;

    void Awake()
    {
        _data.BallAmount = 5;
        _isGamveOver = false;
        _lstBLock = new List<Block>();

        StartCoroutine(GameLoop());
    }
    
    IEnumerator GameLoop()
    {
        yield return StartCoroutine(GameStart());
        yield return StartCoroutine(GamePlaying());
        yield return StartCoroutine(GameEnd());
    }

    IEnumerator GameStart()
    {
        
        EnableMidText();
        SetText("START!");
        yield return new WaitForSeconds(1);
        DisableMidText();
        SpawnBlocks();
    }

    IEnumerator GamePlaying()
    {        
        while(!_isGamveOver)
            yield return null;
    }

    IEnumerator GameEnd()
    {        
        EnableMidText();
        SetText("GAME OVER!");
        yield return new WaitForSeconds(2f);
        DisableMidText();
        CheckHighScore();
        ResetScore();
        SceneManager.LoadScene("GameMenu");
    }

    void EnableMidText() => _midScreenText.enabled = true;
    void DisableMidText() => _midScreenText.enabled = false;
    void SetText(string text) => _midScreenText.text = text;
    void ResetScore() => _score.Score = 0;
    void CheckHighScore()
    {
        if (_score.Score > _score.HighScore)
        {            
            _score.HighScore = _score.Score;
            PlayerPrefs.SetInt("Ball", _score.HighScore);
        }
    }


    public void SpawnBlocks()
    {
        ShiftBlocksDown();
        SpawnRowBlocks(_rowCount);
        _rowCount++;
    }

    void ShiftBlocksDown()
    {
        foreach (var block in _lstBLock)
        {
            if (block)
            {
                block.transform.position = block.transform.position + Vector3.down * _blockDistance;
                if (block.transform.position.y < 0)
                    GameOver();
            }
        }
    }

    void GameOver()
    {
        _isGamveOver = true;
    }

    void SpawnRowBlocks(int rowCount)
    {
        for (int i = 0; i < COLUMN_COUNT; i++)
        {
            if (Random.Range(1, 100) > 15)
            {
                var block = CreateBlock(i, Random.Range(1, 100) < 30);
                var factor = rowCount * 5;
                var min = Mathf.RoundToInt(factor * 0.6f);
                var max = Mathf.RoundToInt(factor * 1.5f);
                block.setHits(Random.Range(min, max));
                block.UpdateBlock();
                _lstBLock.Add(block);
            }                
        }
    }

    Block CreateBlock(int columnCount, bool bonus = false)
    {
        Block block;
        if (bonus)
        {
            block = Instantiate(_bonusPrefab);
        }
        else
        {
            block = Instantiate(_blockPrefab);                  
        }
        block.transform.position = transform.position + Vector3.right * columnCount * _blockDistance;
        return block;      
    }
}
