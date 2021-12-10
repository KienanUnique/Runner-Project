using Script.Player;
using UnityEngine;

namespace Script.Obstacles
{
    public class KillTouch : MonoBehaviour
    {
        private PlayerMovement _playerMovement;

        private void Start()
        {
            _playerMovement = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<PlayerMovement>();
        }
    
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                _playerMovement.Kill();
            }
        }

    }
}
