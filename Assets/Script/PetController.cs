using Pathfinding;
using UnityEngine;

namespace Script
{
    public class PetController : MonoBehaviour
    {
        [SerializeField] private Vector2Int petStartPosition = new Vector2Int(-1, -6);

        private LevelUtilities _levelUtilities;

        private void Start()
        {
            _levelUtilities = GameObject.FindGameObjectWithTag(GameConst.PlayerTag).GetComponent<LevelUtilities>();
        }

        public void Respawn()
        {
            transform.position = _levelUtilities.ConvertCellToWorldPosition((Vector3Int)petStartPosition);
        }

        // TODO: public void AttackToDirection(Vector2 attackDirection)

    }
}
