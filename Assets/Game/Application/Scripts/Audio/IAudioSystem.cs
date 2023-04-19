using UnityEngine;

namespace Game {
    internal interface IAudioSystem {
        void PlayMusic(AudioClip music);
        float CurrentVolume { get; }
        void PlaySound(SFXSound sound);
        void ChangeVolume(float value);
    }
}