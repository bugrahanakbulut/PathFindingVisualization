using System;
using System.Collections;
using UnityEngine;

public class ScaleTween : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform = null;
    
    [SerializeField] private Vector3 _initialScale = Vector3.zero;
    
    [SerializeField] private Vector3 _targetScale = Vector3.one;

    [SerializeField] private float _duration = 0.5f;

    private IEnumerator _playRoutine;

    public void Play(Action callback = null)
    {
        Kill();

        _playRoutine = TweenProgress(_initialScale, _targetScale, callback);

        StartCoroutine(_playRoutine);
    }

    public void PlayReverse(Action callback = null)
    {
        Kill();
        
        _playRoutine = TweenProgress(transform.localScale, _initialScale, callback);

        StartCoroutine(_playRoutine);
    }

    public void Kill()
    {
        if (_playRoutine != null)
        {
            StopCoroutine(_playRoutine);
        }
    }

    private IEnumerator TweenProgress(Vector3 startVal, Vector3 finalVal, Action callback)
    {
        float timePassed = 0;

        while (timePassed < _duration)
        {
            timePassed += Time.deltaTime;

            _targetTransform.localScale = Vector3.Lerp(startVal, finalVal, timePassed / _duration);

            yield return null;
        }
        
        callback?.Invoke();
    }
}
