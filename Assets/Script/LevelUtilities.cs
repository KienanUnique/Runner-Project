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
        [SerializeField] private Tilemap transitionsTilemap;
        [SerializeField] private float slowdownFactor = 0.3f;
        [SerializeField] private Tile twoSidedLineSwitchTile;
        [SerializeField] private Tile leftLineSwitchTile;
        [SerializeField] private Tile rightLineSwitchTile;

        private PlayerController _playerController;
        private PetController _petController;

        private void Start()
        {
            _playerController = GameObject.FindGameObjectWithTag(GameConst.PlayerTag).GetComponent<PlayerController>();
            _petController = GameObject.FindGameObjectWithTag(GameConst.PetTag).GetComponent<PetController>();
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

        public bool IsCellRightOrTwoSidedLineSwitch(Vector3Int cellToCheck)
        {
            var tileToCheck = transitionsTilemap.GetTile<Tile>(cellToCheck);
            return tileToCheck == rightLineSwitchTile || tileToCheck == twoSidedLineSwitchTile;
        }
        
        public bool IsCellLeftOrTwoSidedLineSwitch(Vector3Int cellToCheck)
        {
            var tileToCheck = transitionsTilemap.GetTile<Tile>(cellToCheck);
            return tileToCheck == leftLineSwitchTile || tileToCheck == twoSidedLineSwitchTile;
        }

        public bool IsCellTransition(Vector3Int positionToCheck)
        {
            return transitionsTilemap.HasTile(positionToCheck);
        }

        public bool IsCellBorder(Vector3Int cellToCheck)
        {
            return borderTilemap.HasTile(cellToCheck);
        }

        public Vector3Int ConvertWorldToCellPosition(Vector3 worldPosition)
        {
            return mainGrid.WorldToCell(worldPosition);
        }
        
        public float GetLineCenterInWorld(int gridX)
        {
            return mainGrid.GetCellCenterWorld(new Vector3Int(gridX, 0, 0)).x;
        }
        
        public Vector3 ConvertCellToWorldPosition(Vector3Int gridPosition)
        {
            return mainGrid.GetCellCenterWorld(gridPosition);
        }

        public void RestartLevel()
        {
            _playerController.Respawn();
            _petController.Respawn();
            if (UnityEngine.Camera.main is { })
                UnityEngine.Camera.main.gameObject.GetComponent<CameraFollow>().ReturnCenter();
        }
    }
}
