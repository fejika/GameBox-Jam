using Game;
using Game.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
namespace Game {
    internal class AudioSystem : MonoBehaviour, IAudioSystem {
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] Transform sfxPoolTransform;

        private SFXAudioSourcePool sfxPool;
        private float volumeFactor = 1f;

        private float musicSourceVolume;
        public float CurrentVolume => volumeFactor;

        [Inject]
        public void Construct(TimersFactory tF) {
            sfxPool = new SFXAudioSourcePool(sfxPoolTransform, tF);
        }
        private void Awake() {
            DontDestroyOnLoad(this);
            musicSourceVolume = musicSource.volume;
        }

        public void PlayMusic(AudioClip music) {
            musicSource.Stop();
            musicSource.PlayOneShot(music, musicSourceVolume * volumeFactor);
        }

        public void PlaySound(AudioClip sound) {
            sfxSource.PlayOneShot(sound, sfxSource.volume * volumeFactor);
        }

        public void PlaySound(SFXSound sfx) {
            if (sfx.clip == null) { Debug.LogWarning("SFX Sound is null"); return; }
            float pitch = UnityEngine.Random.Range(sfx.minPitch, sfx.maxPitch);
            sfxPool.PlaySFX(sfx.clip, sfx.volume * volumeFactor, pitch);
        }

        public void ChangeVolume(float value) {
            volumeFactor = value;
        }
    }
}