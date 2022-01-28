using UnityEngine;

namespace Script.Player
{
    public class PlayerEditor : MonoBehaviour
    {
        [Header("Movement")] 
        [SerializeField] private Vector2Int playerStartPosition = new Vector2Int(-1, -5);
        [SerializeField] private int playerDefaultSpeedY = 4;
        [SerializeField] private float playerDefaultSmoothTimeX = 0.05f;

        [Header("Input")] 
        [SerializeField] private float detectSwipeDistance = 0.05f;
        [SerializeField] private float nextTouchWaitSec = 0.4f;
        [SerializeField] private bool debugWithKeyboard = true;

        [Header("Game Object References")] 
        [SerializeField] private GameObject uiController;
        [SerializeField] private GameObject petGameObject;

        public UIController GetUIController()
        {
            return uiController.GetComponent<UIController>();
        }

        public Vector3Int GetPlayerStartPosition()
        {
            return (Vector3Int)playerStartPosition;
        }

        public GameObject GetPetGameObject()
        {
            return petGameObject;
        }

        public PetController GetPetController()
        {
            return petGameObject.GetComponent<PetController>();
        }

        public float GetDetectSwipeDistance()
        {
            return detectSwipeDistance;
        }

        public float GetNextTouchWaitSec()
        {
            return nextTouchWaitSec;
        }

        public bool IsDebugWithKeyboard()
        {
            return debugWithKeyboard;
        }

        public float GetPlayerDefaultSpeedY()
        {
            return playerDefaultSpeedY;
        }

        public float GetPlayerDefaultSmoothTimeX()
        {
            return playerDefaultSmoothTimeX;
        }
    }
}
