using Script.Player;
using UnityEngine;

namespace Script.Obstacles
{
    public class KillTouch : MonoBehaviour
    {
        private PlayerCharacter _playerCharacter;

        private void Start()
        {
            _playerCharacter = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<PlayerCharacter>();
        }
    
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                _playerCharacter.Kill();
            }
        }

    }
}
