using System;
using Constellation.Module_1.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Constellation.Module_1.UI
{
    public class MainScreenUI : MonoBehaviour
    {
        [SerializeField] private Button _showHideButton;
        [SerializeField] private ConstellationAnimator _animator;

        private Text _buttonText;

        private void Awake()
        {
            _buttonText = _showHideButton.GetComponentInChildren<Text>();
        }

        private void Start()
        {
            ChangeButtonLabel();
        }

        private void OnEnable()
        {
            _animator.OnAnimationComplete += OnAnimationCompleteHandler;
            _showHideButton.onClick.AddListener(OnShowHideHandler);
        }

        private void OnDisable()
        {
            _animator.OnAnimationComplete -= OnAnimationCompleteHandler;
            _showHideButton.onClick.RemoveListener(OnShowHideHandler);
        }

        private void OnAnimationCompleteHandler()
        {
            ChangeButtonLabel();
        }

        private void ChangeButtonLabel()
        {
            _buttonText.text = _animator.IsShowing ? "HIDE" : "SHOW";
        }


        private void OnShowHideHandler()
        {
            if (_animator.IsShowing)
            {
                _animator.StartBackwardAnimation();
            }
            else
            {
                _animator.StartForwardAnimation();
            }

            ChangeButtonLabel();
        }
    }
}