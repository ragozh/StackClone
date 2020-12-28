using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PercentageBarSetter : MonoBehaviour
{
    [SerializeField] float _delayTime = 0f;
    [SerializeField] Image foreGround;
    Quaternion _rotation;
    void Awake()
    {
        GetComponentInParent<CharacterController>().OnHealthChange += PercentChanged;
        _rotation = Quaternion.Euler(-(180 - Camera.main.transform.eulerAngles.x), 0, 0);
    }

    void Update()
    {
        transform.rotation = _rotation;
    }

    void PercentChanged(float percent)
    {
        StartCoroutine(ChangePercent(percent));        
    }

    IEnumerator ChangePercent(float pct){
        float preChangedPercent = foreGround.fillAmount;
        float elapsed = 0f;
        while (elapsed < _delayTime) {
            elapsed += Time.deltaTime;
            foreGround.fillAmount = Mathf.Lerp(preChangedPercent, pct, elapsed/_delayTime); // Lerp(a, b, t) calculate number between a, b by the time t
            
            yield return null;
        }
        foreGround.fillAmount = pct;
        foreGround.color = Color.Lerp(Color.red, Color.green,pct);
    }
}
