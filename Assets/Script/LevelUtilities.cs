using Script.Camera;
using Script.Player;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Script
{
    public class LevelUtilities : MonoBehaviour
    {
        [Header("Grid objects")] [SerializeField]
        private Grid mainGrid;

        [SerializeField] private Tilemap borderTilemap;
        [SerializeField] private Tilemap changeLineTilemap;
        [SerializeField] private float slowdownFactor = 0.3f;

        private PlayerController _playerController;

        private void Start()
        {
            _playerController = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<PlayerController>();
        }

        public void SlowDownTime()
        {
            Time.timeScale = slowdownFactor;
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }

        public void NormalizeTime()
        {
            Time.timeScale = 1f;
        }

        public Grid GetMainGrid()
        {
            return mainGrid;
        }

        public Tilemap GetBorderTilemap()
        {
            return borderTilemap;
        }

        public Tilemap GetChangeLineTilemap()
        {
            return changeLineTilemap;
        }

        public int GetLineNumByWorldCoordinate(Vector3 worldCoordinate)
        {
            return mainGrid.WorldToCell(worldCoordinate).x;
        }

        public void RestartLevel()
        {
            _playerController.Respawn();
            if (UnityEngine.Camera.main is { })
                UnityEngine.Camera.main.gameObject.GetComponent<CameraFollow>().ReturnCenter();
        }
    }
}
