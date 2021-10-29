using UnityEngine;

namespace Script
{
    public static class GameConst
    {
        public const int PlayerSpeed = 4;

        public const int LeftDirNum = 0;
        public const int RightDirNum = 1;

        public const int LeftBorderTile = -2;
        public const int RightBorderTile = 2;

        public const string PlayerGameObjName = "Player";

        public static readonly Vector2Int PlayerStartPos = new Vector2Int(-1, -5);
    }
}

