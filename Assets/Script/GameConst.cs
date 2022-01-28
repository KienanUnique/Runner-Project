namespace Script
{
    public static class GameConst
    {
        public const string PlayerGameObjName = "Player";
    }

    public static class InputMode
    {
        public const int Moving = 0;
        public const int Aiming = 1;
    }

    public static class InputEvent
    {
        public const int NoEvent = 0;
        public const int SwipeRight = 1;
        public const int SwipeLeft = 2;
        public const int SwipeUp = 3;
        public const int SwipeDown = 4;
        public const int DoubleTap = 5;
        public const int Aiming = 6;
        public const int FinishedAiming = 7;
    }
}