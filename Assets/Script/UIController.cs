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
        private Vector2 _petOnScreenPosition;

        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = 0;
            foreach (var blackoutGameObject in blackoutGameObjectsOnAiming)
            {
                AddBlackoutMaterialToList(blackoutGameObject);
            }
            
        }

        public void StartDrawingAimiLines(Vector2 screenTouchPosition, Vector2 petOnScreenPosition)
        {
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0,UnityEngine.Camera.main.ScreenToWorldPoint((Vector3)petOnScreenPosition));
            _lineRenderer.SetPosition(1,UnityEngine.Camera.main.ScreenToWorldPoint((Vector3)screenTouchPosition));
            _petOnScreenPosition = petOnScreenPosition;
        }

        public void UpdateAimingTouchPosition(Vector2 screenTouchPosition)
        {
            _lineRenderer.SetPosition(0,UnityEngine.Camera.main.ScreenToWorldPoint((Vector3)_petOnScreenPosition));
            _lineRenderer.SetPosition(1,UnityEngine.Camera.main.ScreenToWorldPoint((Vector3)screenTouchPosition));
        }

        public void StopDrawingAimingLine()
        {
            _lineRenderer.positionCount = 0;
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
