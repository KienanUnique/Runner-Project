using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera mainCamera;
        [SerializeField] private List<GameObject> blackoutGameObjectsOnAiming;
        [SerializeField] private GameObject attackButton;
        [SerializeField] private GameObject restartButton;

        private readonly List<Material> _materialsForBlackoutOnAiming = new List<Material>();
        private static readonly int NeedBlackout = Shader.PropertyToID("_NeedBlackout");
        
        #region Events
        public delegate void RestartButtonPressed();
        public event RestartButtonPressed OnRestartButtonPressed;
        
        public delegate void AttackButtonPressed();
        public event AttackButtonPressed OnAttackButtonPressed;
        #endregion

        private void Start()
        {
            DisableRestartButton();
            
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
        
        public void OnAttackButtonPress(){
            OnAttackButtonPressed.Invoke();
        }
        
        public void OnRestartButtonPress(){
            StopBlackout();
            DisableRestartButton();
            EnableAttackButton();
            OnRestartButtonPressed.Invoke();
        }

        public void ShowRestartScreen()
        {
            EnableRestartButton();
            DisableAttackButton();
            StartBlackout();
        }

        private void EnableAttackButton()
        {
            attackButton.SetActive(true);
        }
        
        private void DisableAttackButton()
        {
            attackButton.SetActive(false);
        }
        
        public void EnableRestartButton()
        {
            restartButton.SetActive(true);
        }
        
        public void DisableRestartButton()
        {
            restartButton.SetActive(false);
        }
    }
}