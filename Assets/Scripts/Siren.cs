using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Siren : MonoBehaviour
{
    [SerializeField] private float _pathTime;
    
    private float _pathRunningTime;
    private float _minVolume;
    private float _maxVolume;
    private float _currentSound;
    private bool _inHouse;
    private AudioSource _audioSource;

    private void OnValidate()
    {
        int minPathTime = 4;

        if (_pathTime < minPathTime)
        {
            _pathTime = minPathTime;
        }
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _minVolume = 0;
        _maxVolume = 1;
        _inHouse = false;
        _currentSound = _audioSource.volume;
    }

    private void Update()
    {
        StopHouseSiren();
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            _pathRunningTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(_currentSound, _maxVolume, _pathRunningTime / _pathTime);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        _pathRunningTime = 0;
        _currentSound = _audioSource.volume;
        
        if (collision.TryGetComponent(out Player player))
        {
            _inHouse = true;
        }

        if (_audioSource.volume <= _minVolume)
        {
            _audioSource.Play();
        }
            
    }

    private void OnTriggerExit(Collider collision)
    {
        _pathRunningTime = 0;
        
        if (collision.TryGetComponent(out Player player))
        {
            _inHouse = false;
            _currentSound = _audioSource.volume;
        }
    }

    private void StopHouseSiren()
    {
        if (_inHouse == false)
        {
            _pathRunningTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(_currentSound, _minVolume, _pathRunningTime / _pathTime);
        }

        if (_audioSource.volume <= _minVolume)
        {
            _audioSource.Stop();
        }
    }
}