using System;
using Constellation.Module_1.Utils;
using UnityEngine;

namespace Constellation.Module_1.Data
{
    public readonly struct EquatorialCoords
    {
        private readonly float _ra;
        private readonly float _dec;

        public EquatorialCoords(float ra, float dec)
        {
            _ra = ra;
            _dec = dec;
        }

        public float Ra => _ra;

        public float Dec => _dec;
    }
}