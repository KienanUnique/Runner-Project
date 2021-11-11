using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Script
{
    public class LevelUtilities : MonoBehaviour
    {
        [Header("Grid objects")]
        [SerializeField] private Grid mainGrid;
        [SerializeField] private Tilemap borderTilemap;
        [SerializeField] private Tilemap changeLineTilemap;

        [Header("Tiles")]
        [SerializeField] private Tile borderTile;
        [SerializeField] private Tile switchLineTile;

        private PlayerUtilities _player;
        private List<Vector3Int> _borderReplacedPositions = new List<Vector3Int>();

        void Start()
        {
            _player = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<PlayerUtilities>();
        }

        public void StartLevel()
        {
            _player.Respawn();
            if (Camera.main is { }) Camera.main.gameObject.GetComponent<CameraFollow>().ReturnCenter();
            foreach (var pos in _borderReplacedPositions)
            {
                borderTilemap.SetTile(pos, null);
                changeLineTilemap.SetTile(pos, switchLineTile);
            }
            _borderReplacedPositions.Clear();
        }

        public void AddBorderOnPosition(Vector3Int borderPlacePosition)
        {
            borderTilemap.SetTile(borderPlacePosition, borderTile);
            changeLineTilemap.SetTile(borderPlacePosition, null);
            _borderReplacedPositions.Add(borderPlacePosition);
        }

        public Tilemap GetLevelBorders()
        {
            return borderTilemap;
        }

        public Grid GetLevelGrid()
        {
            return mainGrid;
        }
    }
}
