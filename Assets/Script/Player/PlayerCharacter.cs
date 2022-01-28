using UnityEngine;

namespace Script.Player
{
    public class PlayerCharacter : MonoBehaviour
    {
        private bool _isAlive = true;

        public bool IsAlive()
        {
            return _isAlive;
        }

        public void Kill()
        {
            _isAlive = false;
        }

        public void RestoreStates()
        {
            _isAlive = true;
        }
    }
}
