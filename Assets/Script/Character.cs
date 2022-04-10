using UnityEngine;

namespace Script
{
    [DefaultExecutionOrder(-1)]
    public class Character : MonoBehaviour
    {
        #region Events

        public delegate void Die();
        public event Die OnDying;

        #endregion
        private bool _isAlive = true;

        public bool IsAlive()
        {
            return _isAlive;
        }

        public void Kill()
        {
            _isAlive = false;
            OnDying?.Invoke();
        }

        public void RestoreStates()
        {
            _isAlive = true;
        }
    }
}
