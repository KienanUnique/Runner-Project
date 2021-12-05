using System.Collections;
using UnityEngine;

namespace Script
{
	public class SwipeInput : MonoBehaviour
	{
		[SerializeField] private float detectSwipeDistance = 0.05f;
		[SerializeField] private float nextTouchWaitSec = 0.4f;
		[SerializeField] private bool debugWithArrowKeys = true;

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
			SwipedRight = false;
			SwipedLeft = false;
			SwipedUp = false;
			SwipedDown = false;
			DoubleTap = false;

			if (Input.touches.Length > 0)
			{
				var t = Input.GetTouch(0);
				if (t.phase == TouchPhase.Began && !_isSwipeDetecting)
				{
					_startPos = new Vector2(t.position.x / Screen.width, t.position.y / Screen.width);
					_isSwipeDetecting = true;
					_swipe = new Vector2(0, 0);
				}
				else if (_isSwipeDetecting && _swipe.magnitude >= detectSwipeDistance)
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

					_wasAlreadyFirstTap = false;
					_isSwipeDetecting = false;
				}
				else if (_isSwipeDetecting && t.phase == TouchPhase.Moved)
				{
					var endPos = new Vector2(t.position.x / Screen.width, t.position.y / Screen.width);
					_swipe = new Vector2(endPos.x - _startPos.x, endPos.y - _startPos.y);
				}
				else if (t.phase == TouchPhase.Ended && _swipe.magnitude < detectSwipeDistance)
				{
					_isSwipeDetecting = false;
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
			}

			if (debugWithArrowKeys)
			{
				SwipedDown = SwipedDown || Input.GetKeyDown(KeyCode.DownArrow);
				SwipedUp = SwipedUp || Input.GetKeyDown(KeyCode.UpArrow);
				SwipedRight = SwipedRight || Input.GetKeyDown(KeyCode.RightArrow);
				SwipedLeft = SwipedLeft || Input.GetKeyDown(KeyCode.LeftArrow);
				DoubleTap = DoubleTap || Input.GetKeyDown(KeyCode.RightShift);
			}
		}
		private IEnumerator NextTouchWaitCooldown()
		{
			yield return new WaitForSeconds(nextTouchWaitSec);
			_wasAlreadyFirstTap = true;
		}
	}
}