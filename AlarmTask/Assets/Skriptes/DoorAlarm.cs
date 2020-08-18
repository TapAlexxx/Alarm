using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource),typeof(Rigidbody2D))]
public class DoorAlarm : MonoBehaviour
{
    [SerializeField] private float _timeToFullAlarmVolume;
    [SerializeField] private ContactFilter2D _filter;

    private AudioSource _audioSource;
    private bool _isThiefInHous;
    private float _currentAlarmDuration;
    private Rigidbody2D _rigidbody2D;

    private readonly RaycastHit2D[] _results = new RaycastHit2D[1];

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_isThiefInHous)
        {
            _currentAlarmDuration += Time.deltaTime;
            float normalizedAlarmVolume = _currentAlarmDuration / _timeToFullAlarmVolume;
            _audioSource.volume = normalizedAlarmVolume;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var collisionCount = _rigidbody2D.Cast(transform.right, _filter, _results);

        if (collisionCount > 0)
        {
            Alarm();
        }
        else
        {
            _isThiefInHous = false;
            _audioSource.Stop();
        }
    }

    private void Alarm()
    {
        _isThiefInHous = true;
        if (!_audioSource.isPlaying)
        {
            _currentAlarmDuration = 0;
            _audioSource.Play();
        }
    }
}
