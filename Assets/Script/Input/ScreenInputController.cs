using System;
using System.Collections;
using Script.Player;
using UnityEngine;

namespace Script.Input
{
	public class ScreenInputController : MonoBehaviour
	{
		private Vector2 _swipe;
		private Vector2 _startPos;

		private int _inputMode;
		private int _inputEvent;
		private Vector2 _aimingGuideVector;

		//private Vector2 _petOnScreenPosition;

		private bool _isSwipeDetecting;
		private bool _wasAlreadyFirstTap;
		private IEnumerator _nextTouchWaitCooldownCoroutine;

		private PlayerEditor _playerEditor;

		private void Start()
		{
			_playerEditor = GetComponent<PlayerEditor>();
			_inputMode = InputMode.Moving;
			_inputEvent = InputEvent.NoEvent;
		}

		private void Update()
		{
			_inputEvent = InputEvent.NoEvent;

			if (UnityEngine.Input.touches.Length > 0)
			{
				var t = UnityEngine.Input.GetTouch(0);
				if (t.phase == TouchPhase.Began && !_isSwipeDetecting)
				{
					_isSwipeDetecting = true;
					StartSwipe(t.position);
				}
				else if (_isSwipeDetecting && _swipe.magnitude >= _playerEditor.GetDetectSwipeDistance())
				{
					_wasAlreadyFirstTap = false;
					_isSwipeDetecting = false;
					DetectSwipeDirection();
				}
				else if (_isSwipeDetecting && t.phase == TouchPhase.Moved)
				{
					UpdateSwipe(t.position);
				}
				else if (t.phase == TouchPhase.Ended && _swipe.magnitude < _playerEditor.GetDetectSwipeDistance())
				{
					_isSwipeDetecting = false;
					OnTapDetected();
				}
			}

			if (_playerEditor.IsDebugWithKeyboard())
			{
				ProcessKeyboardPressedButtons();
			}
		}

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
				_inputEvent = _swipe.x > 0 ? InputEvent.SwipeRight : InputEvent.SwipeLeft;
			}
			else
			{
				_inputEvent = _swipe.y > 0 ? InputEvent.SwipeUp : InputEvent.SwipeDown;
			}
		}

		private void OnTapDetected()
		{
			if (!_wasAlreadyFirstTap)
			{
				_wasAlreadyFirstTap = true;
				_nextTouchWaitCooldownCoroutine = NextTouchWaitCooldown();
				StartCoroutine(_nextTouchWaitCooldownCoroutine);
			}
			else
			{
				StopCoroutine(_nextTouchWaitCooldownCoroutine);
				_wasAlreadyFirstTap = false;
				_inputEvent = InputEvent.DoubleTap;
			}
		}

		private IEnumerator NextTouchWaitCooldown()
		{
			yield return new WaitForSeconds(_playerEditor.GetNextTouchWaitSec());
			_wasAlreadyFirstTap = false;
		}

		private void ProcessKeyboardPressedButtons()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
			{
				_inputEvent = InputEvent.SwipeDown;
			}
			else if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
			{
				_inputEvent = InputEvent.SwipeUp;
			}
			else if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
			{
				_inputEvent = InputEvent.SwipeRight;
			}
			else if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
			{
				_inputEvent = InputEvent.SwipeLeft;
			}
			else if (UnityEngine.Input.GetKeyDown(KeyCode.RightShift))
			{
				_inputEvent = InputEvent.DoubleTap;
			}
		}

		public void EnterAimingMode()
		{
			_inputMode = InputMode.Aiming;
		}

		public void EnterMovingMode()
		{
			_inputMode = InputMode.Moving;
		}

		public bool IsInMovingMode()
		{
			return _inputMode == InputMode.Moving;
		}

		public bool IsInAimingMode()
		{
			return _inputMode == InputMode.Aiming;
		}

		public bool IsAimingMode()
		{
			return _inputMode == InputMode.Aiming;
		}

		public bool IsDoubleTap()
		{
			return _inputEvent == InputEvent.DoubleTap;
		}

		public bool IsHorizontalSwipe()
		{
			return _inputEvent == InputEvent.SwipeLeft || _inputEvent == InputEvent.SwipeRight;
		}

		public int GetHorizontalSwipeDirection()
		{
			return IsHorizontalSwipe() ? _inputEvent : InputEvent.NoEvent;
		}

		public bool IsLeftSwipe()
		{
			return _inputEvent == InputEvent.SwipeLeft;
		}

		public bool IsRightSwipe()
		{
			return _inputEvent == InputEvent.SwipeRight;
		}

		// TODO Vector2 getAimingDirection()
	}
}