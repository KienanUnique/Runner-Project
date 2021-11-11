using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Script.Triggers
{
    public class LineSwitchTrigger : MonoBehaviour
    {
        private Tilemap _borderTilemap;

        private PlayerUtilities _playerUtilities;
        private LevelUtilities _levelUtilities;
        private Grid _mainGrid;
        private Tilemap _transitionsTilemap;

        private Vector3Int _borderPlacePos;

        private void Start()
        {
            _playerUtilities = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<PlayerUtilities>();
            _levelUtilities = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<LevelUtilities>();
            _transitionsTilemap = GetComponent<Tilemap>();
            _borderTilemap = _levelUtilities.GetLevelBorders();
            _mainGrid = _levelUtilities.GetLevelGrid();
        }
    
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
        
            var positionToMove = _mainGrid.WorldToCell(collision.GetContact(0).point);
            if (_mainGrid.GetCellCenterWorld(positionToMove).x > collision.GetContact(0).point.x)
            {
                while (_transitionsTilemap.HasTile(positionToMove))
                {
                    positionToMove.x++;
                }
            }
            else
            {
                while (_transitionsTilemap.HasTile(positionToMove))
                {
                    positionToMove.x--;
                }
            }
            _playerUtilities.MoveToX(_mainGrid.GetCellCenterWorld(positionToMove).x);

            _borderPlacePos = positionToMove;
            _borderPlacePos.x--;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
            while (!_borderTilemap.HasTile(_borderPlacePos))
            {
                _levelUtilities.AddBorderOnPosition(_borderPlacePos);
                _borderPlacePos.y++;
            }
        }
    }
}
