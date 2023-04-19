using Game.Common;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    internal class SFXAudioSourcePool {
        private TimersFactory timersFabric;
        private Transform poolTransform;
        private Dictionary<AudioSource, ITimer> activeAudioSources;
        private List<AudioSource> poolAudioSources;
        public SFXAudioSourcePool(Transform poolTr, TimersFactory tF) {
            poolTransform = poolTr;
            timersFabric = tF;
            poolAudioSources = new List<AudioSource>();
            activeAudioSources = new Dictionary<AudioSource, ITimer>();
            InitializePool();
        }

        public void PlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f) {
            var aS = GetSource();         
            var timer = timersFabric.Create(clip.length);
            activeAudioSources.Add(aS, timer);
            timer.OnCompleted += OnSFXEnd;
            aS.pitch = pitch;
            aS.volume = volume;
            aS.PlayOneShot(clip);
            timer.Start();
            void OnSFXEnd() {
                timer.OnCompleted -= OnSFXEnd;
                timer.Dispose();
                activeAudioSources.Remove(aS);
                poolAudioSources.Add(aS);
                aS.gameObject.SetActive(false);
            }
        }
        private AudioSource GetSource() {
            AudioSource aS;
            if (poolAudioSources.Count == 0)
                aS = AddAudioSourceInPool();
            else
                aS = poolAudioSources[0];
            poolAudioSources.Remove(aS);
            aS.gameObject.SetActive(true);
            return aS;
        }

        private void InitializePool() {
            foreach (Transform tr in poolTransform) {
                var aS = tr.GetComponent<AudioSource>();
                if (aS == null) continue;
                poolAudioSources.Add(aS);
            }
        }
        private AudioSource AddAudioSourceInPool() {
            var go = new GameObject("SFXAudioSource");
            go.transform.parent = poolTransform;
            var aS = go.gameObject.AddComponent<AudioSource>();
            poolAudioSources.Add(aS);
            return aS;
        }

    }
}