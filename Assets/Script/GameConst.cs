namespace Script
{
    public static class GameConst
    {
        public const string PlayerGameObjName = "Player";
    }

    public static class InputModeConst
    {
        public const int Moving = 0;
        public const int Aiming = 1;
    }

    public static class AimingConst
    {
        public const int WaitAiming = 0;
        public const int Aiming = 1;
        public const int FinishedAiming = 2;
    }

    public static class MovingConst
    {
        public const int NoInput = 0;
        public const int SwipedRight = 1;
        public const int SwipedLeft = 2;
        public const int SwipedUp = 3;
        public const int SwipedDown = 4;
        public const int DoubleTap = 5;
    }
}