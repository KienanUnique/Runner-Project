using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> blackoutGameObjectsOnAiming;

        private readonly List<Material> _materialsForBlackoutOnAiming = new List<Material>();
        private static readonly int NeedBlackout = Shader.PropertyToID("_NeedBlackout");

        private LineRenderer _lineRenderer;
        private readonly Vector3[] _aimingLinePoints = new Vector3[2];

        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            foreach (var blackoutGameObject in blackoutGameObjectsOnAiming)
            {
                AddBlackoutMaterialToList(blackoutGameObject);
            }

            _lineRenderer.enabled = false;
        }

        public void StartDrawingAimiLineFromTouchToPet(Vector2 screenTouchPosition, Vector2 petOnScreenPosition)
        {
            _aimingLinePoints[0] = petOnScreenPosition;
            _aimingLinePoints[1] = screenTouchPosition;
            _lineRenderer.SetPositions(_aimingLinePoints);
            _lineRenderer.enabled = true;
        }

        public void UpdateAimingTouchPosition(Vector2 screenTouchPosition)
        {
            _aimingLinePoints[1] = screenTouchPosition;
            _lineRenderer.SetPositions(_aimingLinePoints);
        }

        public void StopDrawingAimingLine()
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
