using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera mainCamera;
        [SerializeField] private List<GameObject> blackoutGameObjectsOnAiming;

        private readonly List<Material> _materialsForBlackoutOnAiming = new List<Material>();
        private static readonly int NeedBlackout = Shader.PropertyToID("_NeedBlackout");

        private LineRenderer _lineRenderer;

        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.enabled = false;
            foreach (var blackoutGameObject in blackoutGameObjectsOnAiming)
            {
                AddBlackoutMaterialToList(blackoutGameObject);
            }
        }

        public void StartDrawingAimiLines(Vector2[] screenPositions)
        {
            _lineRenderer.enabled = true;
            UpdateAimingTouchPosition(screenPositions);
        }

        public void UpdateAimingTouchPosition(Vector2[] screenPositions)
        {
            for (var i = 0; i < screenPositions.Length; i++)
            {
                var newPos = mainCamera.ScreenToWorldPoint(screenPositions[i]);
                newPos.z = 0;
                _lineRenderer.SetPosition(i, newPos);
            }
        }

        public void StopDrawingAimiLines()
        {
            _lineRenderer.enabled = false;
        }

        public void StartBlackout()
        {
            SetBlackoutState(true);
        }

        public void StopBlackout()
        {
            SetBlackoutState(false);
        }

        private void SetBlackoutState(bool isBlackout)
        {
            float valToSet = isBlackout ? 1 : 0;
            foreach (var material in _materialsForBlackoutOnAiming)
            {
                material.SetFloat(NeedBlackout, valToSet);
            }
        }

        private void AddBlackoutMaterialToList(GameObject blackoutGameObject)
        {
            if (blackoutGameObject.GetComponent<Renderer>().material != null)
            {
                _materialsForBlackoutOnAiming.Add(blackoutGameObject.GetComponent<Renderer>().material);
            }
        }
    }
}