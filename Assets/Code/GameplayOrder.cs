using System;
using Enemies;
using UnityEngine;
using UnityEngine.Rendering;

namespace Lag
{
    public class GameplayOrder : MonoBehaviour
    {
        private readonly LaggyInput laggyInput = new();

        public GameSettings GameSettings;

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
            var effects = GameSettings.GetSignalEffects(signalStrength);
            var lag = effects.Lag;
            SignalStrengthIndicator.SetSignalStrength(signalStrength);

            laggyInput.ScanFrameInputs();
            PlayerHud.SetHealth(Bot.Health, Bot.MaxHealth);
            PlayerHud.DisplaySignals(laggyInput, lag);
            PlayerHud.SetLowSignalWarning(effects.LowSignalWarning);
            while (laggyInput.ReceiveNextSignal(lag) is { } signal)
            {
                Bot.SetInput(signal);
            }
            PoorConnectionVolume.weight = effects.PostProcessing;
            MusicMixer.MusicPitch = effects.MusicPitch;
            MusicMixer.NoiseVolume = effects.NoiseVolume;
        }

        public void LateUpdate()
        {
            var effects = GameSettings.GetSignalEffects(signalStrength);

            framesSinceLastRedraw++;
            if (framesSinceLastRedraw > effects.FrameLoss)
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