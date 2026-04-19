using System;
using UnityEngine;

namespace Lag
{
    public class MusicMixer : MonoBehaviour
    {
        public AudioSource Music;
        public AudioSource Noise;
        public float SwitchSpeed = 10;

        [NonSerialized] public float MusicPitch;
        [NonSerialized] public float NoiseVolume;

        public void Update()
        {
            Music.pitch = Mathf.Lerp(Music.pitch, MusicPitch, Time.deltaTime * SwitchSpeed);
            Noise.volume = Mathf.Lerp(Noise.volume, NoiseVolume, Time.deltaTime * SwitchSpeed);
        }
    }
}