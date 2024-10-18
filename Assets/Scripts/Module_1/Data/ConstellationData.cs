using System;
using System.Collections.Generic;
using System.Linq;
using Constellation.Module_1.Utils;
using UnityEngine;

namespace Constellation.Module_1.Data
{
    [Serializable]
    public class ConstellationData : ISerializationCallbackReceiver
    {
        [SerializeField] private string name;
        [SerializeField] private float ra;
        [SerializeField] private float dec;

        [SerializeField] private ConstellationImageData image;

        [SerializeField] private StarsPairData[] pairs;

        [SerializeField] private StarData[] stars;

        [NonSerialized] private readonly Dictionary<int, StarData> _allStars = new Dictionary<int, StarData>();


        public EquatorialCoords CenterCoords => new EquatorialCoords(ra, dec);

        public string Name => name;

        public StarsPairData[] Pairs => pairs;

        public StarData[] Stars => stars;

        public ConstellationImageData Image => image;

        public IReadOnlyDictionary<int, StarData> AllStars => _allStars;

        public bool IsStarInConstellation(int id)
        {
            foreach (var pairData in pairs)
            {
                if (pairData.From == id || pairData.To == id)
                {
                    return true;
                }
            }

            return false;
        }


        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _allStars.Clear();
            foreach (var starData in stars)
            {
                _allStars[starData.ID] = starData;
            }
        }

       
        public int? GetConstellationCenterStar(float radius)
        {
            var constellationStarIds = pairs.SelectMany(data => new int[] { data.From, data.To }).ToHashSet();

            List<StarData> constellationStars = stars.Select(data => data)
                .Where(result => constellationStarIds.Contains(result.ID)).ToList();

            Vector3 total = Vector3.zero;

            foreach (var star in constellationStars)
            {
                var pos = EquatorialMath.ToVector3(star.Coords, radius);
                total += pos;
            }

            var center = total / constellationStars.Count;

            StarData centerStar = null;
            float minDistance = float.MaxValue;

            foreach (var starData in constellationStars)
            {
                var pos = EquatorialMath.ToVector3(starData.Coords, radius);
                var distance = Vector3.Distance(pos, center);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    centerStar = starData;
                }
            }

            return centerStar?.ID;
        }
    }
}