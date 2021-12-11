namespace Script.Input
{
    public class InputState
    {
        private int _inputMode;
        private int _moveInput;
        private int _aimInput;

        public InputState()
        {
            _inputMode = InputModeConst.Moving;
            _moveInput = MovingConst.NoInput;
            _aimInput = AimingConst.WaitAiming;
        }

        public int GetInputMode()
        {
            return _inputMode;
        }

        public void SetInputMode(int newInputMode)
        {
            _inputMode = newInputMode;
        }


        public int GetMoveInput()
        {
            return _moveInput;
        }

        public void SetMoveInput(int newMoveInput)
        {
            _moveInput = newMoveInput;
        }


        public int GetAimInput()
        {
            return _aimInput;
        }

        public void SetAimInput(int newAimInput)
        {
            _aimInput = newAimInput;
        }

        public bool IsDoubleTap()
        {
            return _moveInput == MovingConst.DoubleTap;
        }
        
        public bool IsHorizontalSwipe()
        {
            return _moveInput == MovingConst.SwipedRight || _moveInput == MovingConst.SwipedLeft;
        }
    }
}