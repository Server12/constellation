using UnityEngine;

namespace Constellation.Module_1
{
    [CreateAssetMenu(fileName = "ConstellationAssets", menuName = "Create/Constellation Assets", order = 0)]
    public class ConstellationAssets : ScriptableObject
    {
        [SerializeField] private Texture2D[] _images;

        [SerializeField] private Texture2D _star1;
        [SerializeField] private Texture2D _star2;

        public Texture2D[] Images => _images;

        public Texture2D Star1 => _star1;

        public Texture2D Star2 => _star2;
    }
}