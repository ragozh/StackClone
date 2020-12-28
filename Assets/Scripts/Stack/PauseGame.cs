using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    [SerializeField] Text _midScreenText;
    [SerializeField] FlatSO _flatSO;
    [SerializeField] GameObject _cover;
    bool _isPaused = false;
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && _isPaused)
        {
            Resume();
        }
    }
    public void Pause()
    {
        if (_isPaused) return;
        //Time.timeScale = 0;
        _flatSO.CurrentFlat.Pause();
        _cover.SetActive(true);
        _midScreenText.enabled = true;
        _midScreenText.text = "Game is pause! Tap any where to resume.";
        _isPaused = true;
    }
    public void Resume()
    {
        StartCoroutine(OnResume());
    }

    IEnumerator OnResume()
    {
        _midScreenText.text = "Resume!";
        yield return new WaitForSeconds(0.5f);
        _midScreenText.enabled = false;
        _cover.SetActive(false);
        _isPaused = false;
        _flatSO.CurrentFlat.SetSlideSpeed();
    }
}
