﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace WheleOfFortune
{
    public class WheelOfFortuneController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private WheelOfFortuneReferences _wheelOfFortuneReferences;

        [SerializeField] private Button _spinningButton;
        [SerializeField] private Rigidbody2D _rb;

        [Header("Actions")]
        private UnityAction _onSpinStartEvent;
        private UnityAction<WheelPiece> _onSpinEndEvent;

        [Header("boolean")]
        public bool _isSpinning = false;

        [Header("Coroutine")]
        private float _delayTime = 0.1f;

        private void OnEnable()
        {
            _spinningButton.onClick.AddListener(Spin);
        }

        private void OnDisable()
        {
            _spinningButton.onClick.RemoveListener(Spin);
        }
        public void Spin()
        {
            if (_isSpinning == false)
            {
                _isSpinning = true;
                _onSpinStartEvent?.Invoke();
                _rb.AddTorque(Randomication(_wheelOfFortuneReferences.WheelOfFortuneSettingController.RotatePower));
                StartCoroutine(SpinDelayCoroutine());
            }
        }

        public void OnSpinStart(UnityAction action)
        {
            _onSpinStartEvent = action;
        }

        public void OnSpinEnd(UnityAction<WheelPiece> action)
        {
            _onSpinEndEvent = action;
        }

        private IEnumerator SpinDelayCoroutine()
        {
            yield return new WaitForSeconds(_delayTime);

            StartCoroutine(SpinningCoroutine());
        }

        private IEnumerator SpinningCoroutine()
        {
            while (_rb.angularVelocity > 0f)
            {
                _rb.angularVelocity -= Randomication(_wheelOfFortuneReferences.WheelOfFortuneSettingController.StoppingPower) * Time.fixedDeltaTime;
                _rb.angularVelocity = Mathf.Clamp(_rb.angularVelocity, 0, 1440);

                yield return new WaitForFixedUpdate();
            }

            _rb.angularVelocity = 0;
            WheelPiece selectedPiece = _wheelOfFortuneReferences.PieceGenerator.GetSelectedPiece();
            Debug.Log($"Selected Piece: {selectedPiece.Label}, Amount: {selectedPiece.Multiplier}");

            _onSpinEndEvent?.Invoke(selectedPiece);
            _isSpinning = false;
        }

        private float Randomication(float value)
        {
            float randomValue = Random.Range(value - _wheelOfFortuneReferences.WheelOfFortuneSettingController.RandomizationCoefficient, value + _wheelOfFortuneReferences.WheelOfFortuneSettingController.RandomizationCoefficient);

            return randomValue;
        }
    }
}
   
