using System;
using System.Collections.Generic;
using Constellation.Module_1.Data;
using Constellation.Module_1.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Constellation.Module_1.Logic.Controllers
{
    public class StarsController : MonoBehaviour
    {
        [SerializeField] private Transform _starsHolder;
        [SerializeField] private ConstellationAssets _assets;
        [SerializeField] private Star _starPrefab;

        private readonly Dictionary<int, Star> _starsDict = new Dictionary<int, Star>(20);

        private readonly List<Star> _allStars = new List<Star>(20);

        public IReadOnlyList<Star> AllStars => _allStars;

        public List<Star> GetConstellationStars()
        {
            return _allStars.FindAll(star => star.IsInConstellation);
        }

        public Star GetStar(int id)
        {
            return _starsDict.GetValueOrDefault(id);
        }

        public void Build(ConstellationData data, float radius)
        {
            _allStars.Clear();

            foreach (var starData in data.Stars)
            {
                var starView = _starsDict.GetValueOrDefault(starData.ID);

                if (starView == null)
                {
                    starView = Instantiate(_starPrefab, _starsHolder != null ? _starsHolder : transform);
                    starView.SetId(starData.ID);
                }

                starView.transform.position = EquatorialMath.ToVector3(starData.Coords, radius);
                starView.transform.localScale = Vector3.one / starData.Magnitude;
                starView.Texture = Random.value > 0.5f ? _assets.Star1 : _assets.Star2;
                starView.Color = starData.GetColor();
                starView.gameObject.SetActive(true);

                _starsDict.TryAdd(starData.ID, starView);

                //show constellation stars only
                starView.IsInConstellation = data.IsStarInConstellation(starData.ID);
                if (starView.IsInConstellation)
                {
                    starView.gameObject.SetActive(true);
                }
                else
                {
                    starView.gameObject.SetActive(false);
                }

                _allStars.Add(starView);
            }
        }

        public void UpdateLookToCamera(Camera cam)
        {
            foreach (var star in _allStars)
            {
                star.transform.LookAt(cam.transform, Vector3.up);
            }
        }
    }
}