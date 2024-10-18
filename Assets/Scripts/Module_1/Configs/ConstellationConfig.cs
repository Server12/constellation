using System.Collections.Generic;
using Constellation.Module_1.Data;
using NaughtyAttributes;
using UnityEngine;

namespace Constellation.Module_1
{
    [CreateAssetMenu(fileName = "ConstellationConfig", menuName = "Create/Constellation Config")]
    public class ConstellationConfig : ScriptableObject
    {
        [SerializeField] private float _celestialRadius = 0f;
        
        [SerializeField] private ConstellationData[] items;

        public ConstellationData[] Items => items;

        public float CelestialRadius => _celestialRadius;


        public void ParseJson(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }

#if UNITY_EDITOR

        [SerializeField] private TextAsset _textAsset;

        [Button]
        private void Parse()
        {
            if (_textAsset != null)
            {
                ParseJson(_textAsset.text);
            }
        }

#endif
    }
}