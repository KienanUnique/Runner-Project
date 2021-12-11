using System.Collections;
using UnityEngine;

namespace Script.Input
{
	public class ScreenInputController : MonoBehaviour
	{
		[SerializeField] private float detectSwipeDistance = 0.05f;
		[SerializeField] private float nextTouchWaitSec = 0.4f;
		[SerializeField] private bool debugWithKeyboard = true;

		private Vector2 _swipe;
		private Vector2 _startPos;

		private readonly InputState _inputState = new InputState();
		
		//private Vector2 _petOnScreenPosition;

		private bool _isSwipeDetecting;
		private bool _wasAlreadyFirstTap;
		private IEnumerator _nextTouchWaitCooldownCoroutine;

		public void Update()
		{
			_inputState.SetMoveInput(MovingConst.NoInput);

			if (UnityEngine.Input.touches.Length > 0)
			{
				var t = UnityEngine.Input.GetTouch(0);
				if (t.phase == TouchPhase.Began && !_isSwipeDetecting)
				{
					_isSwipeDetecting = true;
					StartSwipe(t.position);
				}
				else if (_isSwipeDetecting && _swipe.magnitude >= detectSwipeDistance)
				{
					_wasAlreadyFirstTap = false;
					_isSwipeDetecting = false;
					DetectSwipeDirection();
				}
				else if (_isSwipeDetecting && t.phase == TouchPhase.Moved)
				{
					UpdateSwipe(t.position);
				}
				else if (t.phase == TouchPhase.Ended && _swipe.magnitude < detectSwipeDistance)
				{
					_isSwipeDetecting = false;
					OnTapDetected();
				}
			}

			if (debugWithKeyboard)
			{
				ProcessKeyboardPressedButtons();
			}
		}

		public void SwitchIntoAimingInputMode()
		{
			_inputState.SetInputMode(InputModeConst.Aiming);
		}
		
		public void SwitchIntoMovingInputMode()
		{
			_inputState.SetInputMode(InputModeConst.Moving);
		}

		public InputState GetCurrentInputState()
		{
			return _inputState;
		}

		// TODO Vector2 getAimingDirection()

		private void StartSwipe(Vector2 tapPos)
		{
			_startPos = new Vector2(tapPos.x / Screen.width, tapPos.y / Screen.width);
			_swipe = new Vector2(0, 0);
		}

		private void UpdateSwipe(Vector2 tapPos)
		{
			var curEndPos = new Vector2(tapPos.x / Screen.width, tapPos.y / Screen.width);
			_swipe = new Vector2(curEndPos.x - _startPos.x, curEndPos.y - _startPos.y);
		}

		private void DetectSwipeDirection()
		{
			if (Mathf.Abs(_swipe.x) > Mathf.Abs(_swipe.y))
			{
				if (_swipe.x > 0)
				{
					_inputState.SetMoveInput(MovingConst.SwipedRight);
				}
				else
				{
					_inputState.SetMoveInput(MovingConst.SwipedLeft);
				}
			}
			else
			{
				if (_swipe.y > 0)
				{
					_inputState.SetMoveInput(MovingConst.SwipedUp);
				}
				else
				{
					_inputState.SetMoveInput(MovingConst.SwipedDown);
				}
			}
		}

		private void OnTapDetected()
		{
			if (_wasAlreadyFirstTap)
			{
				StopCoroutine(_nextTouchWaitCooldownCoroutine);
				_wasAlreadyFirstTap = false;
				_inputState.SetMoveInput(MovingConst.DoubleTap);
			}
			else
			{
				_wasAlreadyFirstTap = true;
				_nextTouchWaitCooldownCoroutine = NextTouchWaitCooldown();
			}
		}

		private IEnumerator NextTouchWaitCooldown()
		{
			yield return new WaitForSeconds(nextTouchWaitSec);
			_wasAlreadyFirstTap = true;
		}

		private void ProcessKeyboardPressedButtons()
		{
			if (_inputState.GetMoveInput() != MovingConst.NoInput) return;
			
			if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
			{
				_inputState.SetMoveInput(MovingConst.SwipedDown);
			}
			else if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
			{
				_inputState.SetMoveInput(MovingConst.SwipedUp);
			}
			else if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
			{
				_inputState.SetMoveInput(MovingConst.SwipedRight);
			}
			else if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
			{
				_inputState.SetMoveInput(MovingConst.SwipedLeft);
			}
			else if (UnityEngine.Input.GetKeyDown(KeyCode.RightShift))
			{
				_inputState.SetMoveInput(MovingConst.DoubleTap);
			}
		}
	}
}