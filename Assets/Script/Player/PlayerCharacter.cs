using UnityEngine;

namespace Script.Player
{
    [DefaultExecutionOrder(-1)]
    public class PlayerCharacter : MonoBehaviour
    {
        #region Events

        public delegate void PlayerKill();
        public event PlayerKill OnPlayerKill;

        #endregion
        private bool _isAlive = true;

        public bool IsAlive()
        {
            return _isAlive;
        }

        public void Kill()
        {
            _isAlive = false;
            OnPlayerKill?.Invoke();
        }

        public void RestoreStates()
        {
            _isAlive = true;
        }
    }
}
