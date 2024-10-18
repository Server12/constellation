using System;
using System.Linq;
using Constellation.Module_1.Data;
using Constellation.Module_1.Logic.Controllers;
using Constellation.Module_1.Utils;
using NaughtyAttributes;
using UnityEngine;

namespace Constellation.Module_1.Logic
{
    public class ConstellationAnimator : MonoBehaviour
    {
        public event Action OnAnimationComplete;

        private Camera _camera;

        [SerializeField] private Transform _starsHolder;

        [Header("Configs")] [Expandable] [SerializeField]
        private ConstellationConfig _config;

        [Header("Animation")] [Range(0.1f, 1f)] [SerializeField]
        private float _maxImageTransparency = 1f;

        [Range(1f, 20f)] [SerializeField] private float _duration = 2f;

        [ReadOnly] [SerializeField] private float _progress = 0f;

        [Header("Controllers")] [SerializeField]
        private ConstellationImageController _imageController;

        [SerializeField] private StarsController _starsController;
        [SerializeField] private StarsConnectionsController _connectionsController;

        private Vector3 _globalCenter;

        private ConstellationData _constellationData;

        private int _animationDirection;
        private bool _startAnimation;


        [Button(null, EButtonEnableMode.Playmode)]
        public void StartBackwardAnimation()
        {
            IsShowing = false;
            _animationDirection = -1;
            _startAnimation = true;
        }

        [Button(null, EButtonEnableMode.Playmode)]
        public void StartForwardAnimation()
        {
            IsShowing = true;
            _animationDirection = 1;
            _startAnimation = true;
        }

        private void StopAnimation()
        {
            _startAnimation = false;
            _progress = 0f;
            IsShowing = false;
            OnAnimationComplete?.Invoke();
        }

        public bool IsShowing { get; private set; }

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
            BuildConstellation();
        }

        [Button(null, EButtonEnableMode.Playmode)]
        private void BuildConstellation()
        {
            StopAnimation();
            
            _constellationData = _config.Items.FirstOrDefault();
            if (_constellationData != null)
            {
                _globalCenter = EquatorialMath.ToVector3(_constellationData.CenterCoords, _config.CelestialRadius);

                _imageController.Build(_constellationData, _config.CelestialRadius);

                _starsController.Build(_constellationData, _config.CelestialRadius);

                _connectionsController.Build(_constellationData, _starsController.GetConstellationStars(),
                    _duration);
            }
        }


        private void Update()
        {
            _starsController.UpdateLookToCamera(_camera);

            _connectionsController.UpdateLineWidth();


            if (_startAnimation)
            {
                _progress += (Time.deltaTime * _animationDirection) / _duration;
                _progress = Mathf.Clamp01(_progress);

                _imageController.UpdateAnimation(Mathf.Clamp(_progress, 0f, _maxImageTransparency));

                _connectionsController.UpdateAnimation(_progress, _duration);

                if (_progress >= 1f || _progress <= 0)
                {
                    _startAnimation = false;
                    OnAnimationComplete?.Invoke();
                }
            }
        }


        private void LateUpdate()
        {
            _camera.transform.LookAt(_globalCenter, Vector3.up);
        }


        private void OnDrawGizmos()
        {
            if (_config == null) return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _config.CelestialRadius);
        }
    }
}