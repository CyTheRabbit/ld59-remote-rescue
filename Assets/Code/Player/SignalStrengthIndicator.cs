using System.Collections.Generic;
using UnityEngine;

namespace Lag
{
    public class SignalStrengthIndicator : MonoBehaviour
    {
        [SerializeField] private List<Sprite> sprites;
        [SerializeField] private new SpriteRenderer renderer;

        public void SetSignalStrength(int strength)
        {
            renderer.sprite = strength >= sprites.Count ? null : sprites[strength];
        }
    }
}