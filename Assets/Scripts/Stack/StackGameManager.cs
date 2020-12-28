using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StackGameManager : MonoBehaviour
{
    [SerializeField] FlatSO _flatSO;    
    [SerializeField] ScoreSO _score;
    [SerializeField] Text _midScreenText;
    [SerializeField] AudioClip[] _clips;

    AudioSource _audioSource;    
    Spawner _spawnerZ;
    bool _isPlaying;

    void Awake()
    {
        _spawnerZ = GameObject.FindObjectOfType<Spawner>();
        _audioSource = GetComponent<AudioSource>();

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
        yield return new WaitForSeconds(3);
        DisableMidText();
        _isPlaying = true;
        _spawnerZ.Spawn();
    }

    IEnumerator GamePlaying()
    {
        while(_isPlaying)
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

    void ResetScore() => _score.Score = 0;

    void CheckHighScore()
    {
        if (_score.Score > _score.HighScore)
        {            
            _score.HighScore = _score.Score;
            PlayerPrefs.SetInt("Stack", _score.HighScore);
        }
    }

    void EnableMidText() => _midScreenText.enabled = true;
    void DisableMidText() => _midScreenText.enabled = false;
    void SetText(string text) => _midScreenText.text = text;

    public void IsGameOver()
    {
        _isPlaying = false;
        _flatSO.Count = 0;
    }

    void Update()
    {
        if (!_isPlaying) return;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {                
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                OnTap();
        }
    }

    void OnTap()
    {
        if (_flatSO.CurrentFlat == null) return;        
        if (_flatSO.CurrentFlat.Stop())
        {
            _audioSource.clip = _flatSO.IsPerfect ? _clips[0] : _clips[1];
            _audioSource.Play();
            _score.Score++;
            _spawnerZ.Spawn();
        }
    }
}
