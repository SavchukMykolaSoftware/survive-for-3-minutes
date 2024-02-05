using System.Collections.Generic;
using UnityEngine;

public class AudioPauser : MonoBehaviour
{
    [SerializeField] private List<AudioSource> AudioSourcesWhichCanBePaused = new();
    
    private List<AudioSource> PausedAusioSources = new();

    public void AddAudioSourceToRegister(AudioSource audioSourceToBeAdded)
    {
        AudioSourcesWhichCanBePaused.Add(audioSourceToBeAdded);
    }

    public void PauseAudio()
    {
        PausedAusioSources.Clear();
        foreach (AudioSource oneAudioSource in AudioSourcesWhichCanBePaused)
        {
            if (oneAudioSource != null && oneAudioSource.isPlaying)
            {
                oneAudioSource.Pause();
                PausedAusioSources.Add(oneAudioSource);
            }
        }
    }

    public void ResumeAudio()
    {
        foreach (AudioSource oneAudioSource in PausedAusioSources)
        {
            if (oneAudioSource != null)
            {
                oneAudioSource.Play();
            }
        }
    }
}