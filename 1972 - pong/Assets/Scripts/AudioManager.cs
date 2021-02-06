using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource = default;
    [SerializeField] private List<AudioClip> _audioClips = default;

    private Dictionary<int, int> _audiClipsDict = default;

    void Start()
    {
        _audiClipsDict = new Dictionary<int, int>();

        for (int i = 0; i < _audioClips.Count; i++)
        {
            _audiClipsDict.Add(_audioClips[i].name.GetHashCode(), i);
        }
    }

    public void PlaySound(string clipName)
    {
        var clipKey = clipName.GetHashCode();
        if (!_audiClipsDict.ContainsKey(clipKey))
        {
            Debug.LogWarning($"Clip {clipName} does not exists!");
            return;
        }
        
        _audioSource.clip = _audioClips[_audiClipsDict[clipKey]];
        _audioSource.Play();
    }
}
