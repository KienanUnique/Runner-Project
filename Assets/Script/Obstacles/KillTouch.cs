using UnityEngine;

namespace Script.Obstacles
{
    public class KillTouch : MonoBehaviour
    {
        private PlayerUtilities _playerUtilities;

        private void Start()
        {
            _playerUtilities = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<PlayerUtilities>();
        }
    
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                _playerUtilities.Kill();
            }
        }

    }
}
