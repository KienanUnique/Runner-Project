using System;
using UnityEngine;

namespace Script
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float lerpSpeed = 1.0f;
        public Boolean isCentered = true;
        public float cameraMiddleX = 0.48f;

        private Vector3 _offset;
        private Vector3 _targetPos;

        private void Start()
        {
            if (target == null) return;

            _offset = transform.position - target.position;
        }

        private void Update()
        {
            if (target == null) return;

            if (isCentered)
            {
                //targetPos = target.position + offset;
                //_targetPos.y = target.position.y + _offset.y;
                //_targetPos.z = target.position.z + _offset.z;
                _targetPos.x = cameraMiddleX;
                _targetPos = new Vector3(cameraMiddleX, target.position.y + _offset.y, _offset.z);
            }

            else
            {
                _targetPos = target.position + _offset;
            }

            transform.position = Vector3.Lerp(transform.position, _targetPos, lerpSpeed * Time.deltaTime);
        }

    }
}
