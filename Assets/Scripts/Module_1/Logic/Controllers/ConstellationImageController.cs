using System.Linq;
using Constellation.Module_1.Data;
using Constellation.Module_1.Utils;
using UnityEngine;

namespace Constellation.Module_1.Logic.Controllers
{
    public class ConstellationImageController : MonoBehaviour
    {
        [SerializeField] private ConstellationAssets _assets;

        [SerializeField] private ConstellationImage _imageInstance;

       // [Range(0f, 10f)] [SerializeField] private float _maxConstellationImageBrightness = 1f;
        

        public void Build(ConstellationData data, float radius)
        {
            var imageData = data.Image;

            _imageInstance.Texture =
                _assets.Images.FirstOrDefault(texture2D => texture2D.name == data.Name);
            _imageInstance.Alpha = 0f;

            var center = EquatorialMath.ToVector3(imageData.CenterCoords, radius);
            _imageInstance.transform.localScale = Vector3.one * imageData.Scale;
            _imageInstance.transform.rotation = Quaternion.LookRotation(center) *
                                                Quaternion.Euler(0f, 0f, imageData.Angle);

            _imageInstance.transform.position = center;
        }

        public void UpdateAnimation(float progress)
        {
            
            _imageInstance.Alpha = Mathf.Clamp01(progress);
        }
    }
}