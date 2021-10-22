using UnityEngine;

namespace Script
{
    public class KillTouch : MonoBehaviour
    {
        private PlayerUtilities _player;

        private void Start()
        {
            //_player = GameObject.Find("Player").GetComponent<PlayerUtilities>();
            _player = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<PlayerUtilities>();
        }
    
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                _player.Kill();
            }
        }

    }
}
