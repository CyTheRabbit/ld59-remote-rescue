using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Lag
{
    public class RadioIndicator : MonoBehaviour
    {
        [SerializeField] private Color upColor;
        [SerializeField] private Color downColor;
        [SerializeField] private Image[] bars;

        public void DisplaySignals(LaggyInput input, Bot bot, float lag)
        {
            if (lag <= 0) { lag = 0.01f; }
            var barStrengths = new NativeArray<float>(bars.Length, Allocator.Temp);
            var now = Time.time;

            if (bot.Signal is not { Move: false, Fire: false })
            {
                var nextTime = input.SignalQueue.Count > 0 ? input.SignalQueue[0].Time : now;
                FillInterval(
                    Mathf.InverseLerp(now - lag, now, now - lag) * bars.Length,
                    Mathf.InverseLerp(now - lag, now, nextTime) * bars.Length,
                    barStrengths);
            }
            for (var i = 0; i < input.SignalQueue.Count; i++)
            {
                var signal = input.SignalQueue[i];
                if (signal is { Move: false, Fire: false }) { continue; }

                var nextTime = i + 1 < input.SignalQueue.Count ? input.SignalQueue[i + 1].Time : now;
                FillInterval(
                    Mathf.InverseLerp(now - lag, now, signal.Time) * bars.Length,
                    Mathf.InverseLerp(now - lag, now, nextTime) * bars.Length,
                    barStrengths);
            }

            for (var i = 0; i < bars.Length; i++)
            {
                bars[i].color = Color.Lerp(downColor, upColor, barStrengths[i]);
            }
        }

        private void FillInterval(float start, float end, NativeArray<float> barStrengths)
        {
            var startI = Mathf.FloorToInt(start);
            var endI = Mathf.FloorToInt(end);

            if (startI >= bars.Length) { startI = bars.Length - 1; }
            if (startI < 0) { startI = 0; }
            if (endI >= bars.Length) { endI = bars.Length - 1; }
            if (endI < 0) { endI = 0; }

            if (startI == endI)
            {
                barStrengths[startI] += end - start;
                return;
            }

            barStrengths[startI] += startI - start + 1;
            barStrengths[endI] += end - endI;
            for (var i = startI + 1; i < endI; i++)
            {
                barStrengths[i] += 1;
            }
        }
    }
}