using UnityEngine;

namespace Lag
{
    public class MusicMixer : MonoBehaviour
    {
        public AudioSource Music;
        public AudioSource Noise;
        public float PitchFactor;
        public float SwitchSpeed = 10;

        private int pitchPower;
        private float noisePower;

        public void Update()
        {
            var targetPitch = Mathf.Pow(PitchFactor, pitchPower);
            Music.pitch = Mathf.Lerp(Music.pitch, targetPitch, Time.deltaTime * SwitchSpeed);
            Noise.volume = Mathf.Lerp(Noise.volume, noisePower, Time.deltaTime * SwitchSpeed);
        }

        public void SetSignalStrength(int strength)
        {
            pitchPower = strength / 3;
            noisePower = Mathf.Clamp((strength - 4) / 4f, 0, 1);
        }
    }
}