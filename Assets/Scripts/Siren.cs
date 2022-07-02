using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Siren : MonoBehaviour
{
    [SerializeField] private float _timeSoundGain;
    
    private float _soundRunningTime;
    private float _minVolume;
    private float _maxVolume;
    private float _currentSound;
    private AudioSource _audioSource;
    private Coroutine _stopByTimerJob;
    private Coroutine _playByTimerJob;

    private void OnValidate()
    {
        int minPathTime = 4;

        if (_timeSoundGain < minPathTime)
        {
            _timeSoundGain = minPathTime;
        }
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _minVolume = 0;
        _maxVolume = 1;
        _currentSound = _audioSource.volume;
    }

    private void OnTriggerEnter(Collider collision)
    {
        _soundRunningTime = 0;
        _currentSound = _audioSource.volume;
        StopSoundCoroutines();
        
        if (collision.TryGetComponent(out Player player))
        {
            if (_currentSound <= _minVolume)
            {
                _audioSource.Play();
            }
            
            _playByTimerJob = StartCoroutine(PlayByTimer());
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        _soundRunningTime = 0;
        _currentSound = _audioSource.volume;
        StopSoundCoroutines();
        
        if (collision.TryGetComponent(out Player player))
        {
            _stopByTimerJob = StartCoroutine(StopByTimer());
        }
    }

    private void StopSoundCoroutines()
    {
        if (_playByTimerJob != null)
        {
            StopCoroutine(_playByTimerJob);
        }

        if (_stopByTimerJob != null)
        {
            StopCoroutine(_stopByTimerJob);
        }
    }

    private IEnumerator PlayByTimer()
    {
        while (_audioSource.volume < _maxVolume)
        {
            _soundRunningTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(_currentSound, _maxVolume, _soundRunningTime / _timeSoundGain);
            
            yield return null;
        }
    }

    private IEnumerator StopByTimer()
    {
        while (_audioSource.volume > _minVolume)
        {
            _soundRunningTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(_currentSound, _minVolume, _soundRunningTime / _timeSoundGain);
            
            yield return null;
        }
        
        _audioSource.Stop();
    }
}