using UnityEngine;
using UnityEngine.Tilemaps;

namespace Script
{
    public class LevelUtilities : MonoBehaviour
    {
        [Header("Grid objects")]
        [SerializeField] private Vector2Int playerStartPos = new Vector2Int(-1, -5);
        [SerializeField] private Grid mainGrid;
        [SerializeField] private Tilemap borderTilemap;
        [SerializeField] private Tilemap changeLineTilemap;
        

        private PlayerUtilities _player;

        void Start()
        {
            _player = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<PlayerUtilities>();
        }

        public void StartLevel()
        {
            _player.Respawn();
            if (Camera.main is { }) Camera.main.gameObject.GetComponent<CameraFollow>().ReturnCenter();
        }

        public Tilemap GetLevelBorders()
        {
            return borderTilemap;
        }
        
        public Tilemap GetLevelLineSwitches()
        {
            return changeLineTilemap;
        }

        public Grid GetLevelGrid()
        {
            return mainGrid;
        }
        
        public Vector3Int GetPlayerGridStartPos()
        {
            return (Vector3Int)playerStartPos;
        }
    }
}
