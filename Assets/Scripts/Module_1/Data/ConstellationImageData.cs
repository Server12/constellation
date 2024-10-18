using System;
using UnityEngine;

namespace Constellation.Module_1.Data
{
    [Serializable]
    public class ConstellationImageData
    {
        [SerializeField] private float scale;
        [SerializeField] private float angle;

        [SerializeField] private float ra;
        [SerializeField] private float dec;

        public float Scale => scale;

        public EquatorialCoords CenterCoords => new EquatorialCoords(ra, dec);
        
        public float Angle => angle;
    }
}