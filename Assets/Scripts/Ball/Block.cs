using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] ScoreSO _score;
    [SerializeField] BallSO _data;
    [SerializeField] bool _isBonus;
    AudioSource _audioSource;
    int _hits = 50;
    SpriteRenderer _spriteRender;
    TextMeshPro _text;

    void Awake()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
        if (_isBonus)   return;
        _spriteRender = GetComponent<SpriteRenderer>();
        _text = GetComponentInChildren<TextMeshPro>();

        UpdateBlock();
    }
    public void UpdateBlock()
    {
        if (_isBonus)   return;
        _text?.SetText(_hits.ToString());
        _spriteRender.color = Color.Lerp(Color.red, Color.green, (float)_hits / 100);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!_isBonus)
        {
            BlockHit();
            other.gameObject.GetComponent<Ball>().BallScore();
        }
        else
        {
            BonusHit();
        }
    }

    void BonusHit()
    {
        _data.BallAmount++;
        _audioSource.transform.SetParent(null);
        _audioSource.Play();
        GameObject.Destroy(_audioSource.gameObject, 1);
        GameObject.Destroy(gameObject);
    }

    void BlockHit()
    {
        _score.Score++;
        _hits--;
        _audioSource.Play();
        if (_hits > 0)
            UpdateBlock();
        else
            GameObject.Destroy(gameObject);
    }

    public void setHits(int hits) => _hits = hits;
}
