using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Unity.Mathematics;

public class Fade : MonoBehaviour
{
    public GameObject finalScoreOn;    

    [SerializeField] private float _fadeTime = 1.5f;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _respawnPoint;
    public static int deadnum;

    private Image _image;
    private CinemachineVirtualCamera _virtualCam;

    private void Awake() {
        _image = GetComponent<Image>();
        _virtualCam = FindFirstObjectByType<CinemachineVirtualCamera>();
    }

    public void FadeInAndOut() {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn() {
        yield return StartCoroutine(FadeRoutine(1f));
        Respawn();
        StartCoroutine(FadeRoutine(0f));
    }
    
    private IEnumerator FadeRoutine(float targetAlpha) {
        float elapsedTime = 0f;
        float startValue = _image.color.a;

        while (elapsedTime < _fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetAlpha, elapsedTime / _fadeTime);
            _image.color = new Color(_image.color.r, _image.color.b, _image.color.g, newAlpha);
            yield return null;
        }

        _image.color = new Color(_image.color.r, _image.color.b, _image.color.g, targetAlpha);
    }

    private void Respawn() {
        deadnum++;
        if(deadnum>=3)
        {
            finalScoreOn.SetActive(true);
            Time.timeScale = 0;

            

            deadnum=0;
            Debug.Log("3 Kere öldün!!");



        }

        Transform player = Instantiate(_playerPrefab, _respawnPoint.position, Quaternion.identity).transform;
        _virtualCam.Follow = player;
    }
}
