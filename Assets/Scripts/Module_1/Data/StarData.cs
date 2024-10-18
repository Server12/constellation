using System;
using Constellation.Module_1.Utils;
using UnityEngine;

namespace Constellation.Module_1.Data
{
    [Serializable]
    public class StarData
    {
        [SerializeField] private int id;
        [SerializeField] private float ra;
        [SerializeField] private float dec;
        [SerializeField] private float magnitude;
        [SerializeField] private string color;

        public int ID => id;

        public EquatorialCoords Coords => new EquatorialCoords(ra, dec);

        public float Magnitude => magnitude;

        public Vector3 GetWorldPosition(float radius)
        {
            return EquatorialMath.ToVector3(ra, dec, radius);
        }

        public Color GetColor()
        {
            if (ColorUtility.TryParseHtmlString($"#{color}", out var c))
            {
                return c;
            }

            return Color.white;
        }
    }
}