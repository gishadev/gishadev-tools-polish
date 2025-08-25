using System;

namespace gishadev.tools.Audio
{
    public interface IAudioManager
    {
        event Action<AudioData> AudioStarted;
        void SetSFXVolume(float volumePercent);
        void SetMusicVolume(float volumePercent);
        
        void PlaySFX(int index); 
        void PlayMusic(int index);
    }
}