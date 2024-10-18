using UnityEngine;

namespace Constellation.Module_1.Components
{
    public class MaterialUpdater : MonoBehaviour
    {
        private static readonly int BaseMapShaderProp = Shader.PropertyToID("_BaseMap");
        private static readonly int ColorShaderProp = Shader.PropertyToID("_BaseColor");
        private static readonly int AlphaShaderProp = Shader.PropertyToID("_Alpha");
        private static readonly int BrightnessShaderProp = Shader.PropertyToID("_Brightness");

        [SerializeField] private Renderer _renderer;


        public Texture2D GetTexture()
        {
            return (Texture2D)_renderer.material.GetTexture(BaseMapShaderProp);
        }

        public void SetTexture(Texture2D texture2D)
        {
            _renderer.material.SetTexture(BaseMapShaderProp, texture2D);
        }


        public void SetAlpha(float value)
        {
            _renderer.material.SetFloat(AlphaShaderProp, Mathf.Clamp01(value));
        }

        public float GetAlpha()
        {
            return _renderer.material.GetFloat(AlphaShaderProp);
        }

        public void SetBrightness(float value)
        {
            var range = 10f * Mathf.Clamp01(value);
            _renderer.material.SetFloat(BrightnessShaderProp, range);
        }

        public float GetBrightness()
        {
            var value = _renderer.material.GetFloat(BrightnessShaderProp);
            return value / 10f;
        }

        public Color GetColor()
        {
            return _renderer.material.GetColor(ColorShaderProp);
        }

        public void SetColor(Color32 color32)
        {
            _renderer.material.SetColor(ColorShaderProp, color32);
            
        }
    }
}