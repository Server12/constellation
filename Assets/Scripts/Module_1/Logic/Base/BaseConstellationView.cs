using Constellation.Module_1.Components;
using UnityEngine;

namespace Constellation.Module_1.Logic
{
    public abstract class BaseConstellationView : MonoBehaviour
    {
        [SerializeField] private MaterialUpdater _materialUpdater;

        public Texture2D Texture
        {
            get => _materialUpdater.GetTexture();
            set => _materialUpdater.SetTexture(value);
        }

        public float Alpha
        {
            get => _materialUpdater.GetAlpha();
            set => _materialUpdater.SetAlpha(value);
        }

        public float Brightness
        {
            get => _materialUpdater.GetBrightness();
            set => _materialUpdater.SetBrightness(value);
        }

        public Color Color
        {
            get => _materialUpdater.GetColor();
            set => _materialUpdater.SetColor(value);
        }
    }
}