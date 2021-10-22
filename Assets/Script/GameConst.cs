using UnityEngine;

namespace Script
{
    public static class GameConst
    {
        public const int s_Speed = 4;

        public const int s_LeftDir = 0;
        public const int s_RightDir = 1;

        public const int s_LeftBorder = -2;
        public const int s_RightBorder = 1;

        public const string PlayerGameObjName = "Player";

        public static readonly Vector2Int PlayerStartPos = new Vector2Int(-1, -5);
    }
}

