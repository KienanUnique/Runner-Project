using UnityEngine;

namespace Script.Player
{
    public class PlayerEditor : MonoBehaviour
    {
        [Header("Game Object References")] [SerializeField]
        private GameObject uiController;

        [SerializeField] private GameObject petGameObject;

        public UIController GetUIController()
        {
            return uiController.GetComponent<UIController>();
        }

        public PetController GetPetController()
        {
            return petGameObject.GetComponent<PetController>();
        }
    }
}