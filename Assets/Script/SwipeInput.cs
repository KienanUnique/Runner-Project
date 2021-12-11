using System.Collections;
using UnityEngine;

namespace Script
{
	public class SwipeInput : MonoBehaviour
	{
		[SerializeField] private float detectSwipeDistance = 0.05f;
		[SerializeField] private float nextTouchWaitSec = 0.4f;
		[SerializeField] private bool debugWithKeyboard = true;

		public static bool SwipedRight;
		public static bool SwipedLeft;
		public static bool SwipedUp;
		public static bool SwipedDown;
		public static bool DoubleTap;

		private Vector2 _swipe;
		private Vector2 _startPos;

		private bool _isSwipeDetecting;
		private bool _wasAlreadyFirstTap;
		private IEnumerator _nextTouchWaitCooldownCoroutine;

		public void Update()
		{
			SwitchInputFlagsToDefaultState();
			
			if (Input.touches.Length > 0)
			{
				var t = Input.GetTouch(0);
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

		private void SwitchInputFlagsToDefaultState()
		{
			SwipedRight = false;
			SwipedLeft = false;
			SwipedUp = false;
			SwipedDown = false;
			DoubleTap = false;
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
				if (_swipe.x > 0)
				{
					SwipedRight = true;
				}
				else
				{
					SwipedLeft = true;
				}
			}
			else
			{
				if (_swipe.y > 0)
				{
					SwipedUp = true;
				}
				else
				{
					SwipedDown = true;
				}
			}
		}
		
		private void OnTapDetected()
		{
			if (_wasAlreadyFirstTap)
			{
				StopCoroutine(_nextTouchWaitCooldownCoroutine);
				_wasAlreadyFirstTap = false;
				DoubleTap = true;
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
			SwipedDown = SwipedDown || Input.GetKeyDown(KeyCode.DownArrow);
			SwipedUp = SwipedUp || Input.GetKeyDown(KeyCode.UpArrow);
			SwipedRight = SwipedRight || Input.GetKeyDown(KeyCode.RightArrow);
			SwipedLeft = SwipedLeft || Input.GetKeyDown(KeyCode.LeftArrow);
			DoubleTap = DoubleTap || Input.GetKeyDown(KeyCode.RightShift);
		}
	}
}