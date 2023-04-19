using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


namespace Game {
    [Serializable]
    internal struct SFXSound {
        [SerializeField] public string id;
        [SerializeField] public AudioClip clip;
        [SerializeField, Range(0, 1)] public float volume;
        [SerializeField, Range(0, 1)] public float minPitch;
        [SerializeField, Range(1, 2)] public float maxPitch;
    }
}