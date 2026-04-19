using System;
using Enemies;
using Lag;
using UnityEngine;
using UnityEngine.Rendering;

namespace Code
{
    public class GameplayOrder : MonoBehaviour
    {
        private readonly LaggyInput laggyInput = new();

        public float[] LagGradation;

        public Camera LaggyCamera;
        public MusicMixer MusicMixer;
        public Bot Bot;
        public SignalStrengthIndicator SignalStrengthIndicator;
        public PlayerHud PlayerHud;
        public Volume PoorConnectionVolume;

        private SignalSource[] signalSources;
        private Jammer[] jammers;
        private int signalStrength;
        private int framesSinceLastRedraw;

        public void Start()
        {
            signalSources = FindObjectsByType<SignalSource>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            jammers = FindObjectsByType<Jammer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            Application.targetFrameRate = 60;
        }

        public void Update()
        {
            signalStrength = GetSignalStrength();
            var lag = signalStrength >= LagGradation.Length ? LagGradation[^1] : LagGradation[signalStrength];
            SignalStrengthIndicator.SetSignalStrength(signalStrength);

            laggyInput.ScanFrameInputs();
            PlayerHud.SetHealth(Bot.Health, Bot.MaxHealth);
            PlayerHud.DisplaySignals(laggyInput, lag, signalStrength);
            while (laggyInput.ReceiveNextSignal(lag) is { } signal)
            {
                Bot.SetInput(signal);
            }
            PoorConnectionVolume.weight = Mathf.InverseLerp(4, 12, signalStrength);
            MusicMixer.SetSignalStrength(signalStrength);
        }

        public void LateUpdate()
        {
            var framesToRedraw = signalStrength;
            framesSinceLastRedraw++;
            if (framesSinceLastRedraw > framesToRedraw)
            {
                LaggyCamera.Render();
                framesSinceLastRedraw = 0;
            }
        }

        private int GetSignalStrength()
        {
            var botPosition = Bot.transform.position;
            var signal = 12;
            foreach (var source in signalSources)
            {
                if (!source.isActiveAndEnabled) { continue; }
                signal = Mathf.Min(signal, source.GetSignalStrengthAtPosition(botPosition));
            }
            var jam = 0;
            foreach (var jammer in jammers)
            {
                if (!jammer.isActiveAndEnabled) { continue; }
                jam = Math.Max(jam, jammer.GetJamStrengthAtPosition(botPosition));
            }
            return Math.Max(signal, jam);
        }
    }
}