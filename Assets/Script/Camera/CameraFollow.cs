using System;
using UnityEngine;

namespace Script
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float lerpSpeed = 1.0f;
        [SerializeField] private Boolean isCentered = true;
        [SerializeField] private float cameraMiddleX = 0.5f;
        
        private float _currentCameraMiddleX;
        private Vector3 _offset;
        private Vector3 _targetPos;

        private void Start()
        {
            if (target == null) return;
            _currentCameraMiddleX = cameraMiddleX;
            _offset = transform.position - target.position;
        }

        private void Update()
        {
            if (target == null) return;

            if (isCentered)
            {
                _targetPos = new Vector3(_currentCameraMiddleX, target.position.y + _offset.y, _offset.z);
            }

            else
            {
                _targetPos = target.position + _offset;
            }

            transform.position = Vector3.Lerp(transform.position, _targetPos, lerpSpeed * Time.deltaTime);
        }

        public void SetCenter(float centerX)
        {
            _currentCameraMiddleX = centerX;
        }

        public void ReturnCenter()
        {
            _currentCameraMiddleX = cameraMiddleX;
        }
    }
}
