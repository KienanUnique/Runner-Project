using UnityEngine;

namespace Script
{
    public class LevelUtilities : MonoBehaviour
    {
        private PlayerUtilities _player;
        void Start()
        {
            _player = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<PlayerUtilities>();
        }

        public void StartLevel()
        {
            _player.Respawn();
            Camera.main.gameObject.GetComponent<CameraFollow>().ReturnCenter();
        }
    }
}
