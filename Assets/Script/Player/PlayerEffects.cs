using System.Collections.Generic;
using UnityEngine;

namespace Script.Player
{
    public class PlayerEffects : MonoBehaviour
    {
        [SerializeField] private List<GameObject> blackoutGameObjectsOnAiming;

        private readonly List<Material> _materialsForBlackoutOnAiming = new List<Material>();
        private static readonly int NeedBlackout = Shader.PropertyToID("_NeedBlackout");
    
        private bool _curBlackoutState;

        void Start()
        {
            foreach (var blackoutGameObject in blackoutGameObjectsOnAiming)
            {
                AddBlackoutMaterialToList(blackoutGameObject);
            }

            _curBlackoutState = false;

        }

        public void SetBlackout(bool needBlackout)
        {
            _curBlackoutState = needBlackout;
            float valToSet = needBlackout ? 1 : 0;
            foreach (var material in _materialsForBlackoutOnAiming)
            {
                material.SetFloat(NeedBlackout, valToSet);
            }
        }

        public bool GetBlackoutState()
        {
            return _curBlackoutState;
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
