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

        private void Start()
        {
            foreach (var blackoutGameObject in blackoutGameObjectsOnAiming)
            {
                AddBlackoutMaterialToList(blackoutGameObject);
            }
        }

        public void StartBlackout()
        {
            SetBlackoutState(true);
        }

        public void StopBlackout()
        {
            SetBlackoutState(false);
        }

        public Vector3 ScreenTouchToWorldPoint(Vector2 touchOnScreenPosition)
        {
            return mainCamera.ScreenToWorldPoint(touchOnScreenPosition);
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