using System.Collections;
using UnityEngine;

namespace Script
{
	public class SwipeInput : MonoBehaviour
	{

		[SerializeField] private const float DetectSwipeDistance = 0.05f;
		[SerializeField] private const float NextTouchWaitSec = 0.8f;
		[SerializeField] private bool debugWithArrowKeys = true;

		[HideInInspector] public static bool SwipedRight = false;
		[HideInInspector] public static bool SwipedLeft = false;
		[HideInInspector] public static bool SwipedUp = false;
		[HideInInspector] public static bool SwipedDown = false;
		[HideInInspector] public static bool DoubleTap = false;

		private Vector2 _swipe;
		private Vector2 _startPos;

		private bool _isSwipeDetecting = false;
		//private int _touchesCount = 0;
		private bool _wasFirstTap = false;
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
				Touch t = Input.GetTouch(0);
				if (t.phase == TouchPhase.Began && !_isSwipeDetecting)
				{
					_startPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
					_isSwipeDetecting = true;
					_swipe = new Vector2(0, 0);
				}
				else if (_isSwipeDetecting && _swipe.magnitude >= DetectSwipeDistance)
				{
					if (Mathf.Abs(_swipe.x) > Mathf.Abs(_swipe.y))
					{
						// Horizontal swipe
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
						// Vertical swipe
						if (_swipe.y > 0)
						{
							SwipedUp = true;
						}
						else
						{
							SwipedDown = true;
						}
					}

					_wasFirstTap = false;
					_isSwipeDetecting = false;
				}
				else if (_isSwipeDetecting && t.phase == TouchPhase.Moved)
				{
					Vector2 endPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
					_swipe = new Vector2(endPos.x - _startPos.x, endPos.y - _startPos.y);
				}
				else if (t.phase == TouchPhase.Ended && _swipe.magnitude < DetectSwipeDistance)
				{
					if (_wasFirstTap)
					{
						StopCoroutine(_nextTouchWaitCooldownCoroutine);
						_wasFirstTap = false;
						DoubleTap = true;
					}
					else
					{
						_wasFirstTap = true;
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
			yield return new WaitForSeconds(NextTouchWaitSec);
			_wasFirstTap = true;
		}
	}
}