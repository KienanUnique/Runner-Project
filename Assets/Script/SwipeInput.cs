using UnityEngine;

/*
 * Swipe Input script for Unity by @fonserbc, free to use wherever
 * https://github.com/MarcoMig/Unity-Swipe-Detection/blob/master/Unity%20Swipe%20Detection.cs
 * Attack to a gameObject, check the static booleans to check if a swipe has been detected this frame
 * Eg: if (SwipeInput.swipedRight) ...
 *
 * 
 */

public class SwipeInput : MonoBehaviour {
	
	public const float DETECT_SWIPE_DISTANCE = 0.05f;

	public static bool swipedRight = false;
	public static bool swipedLeft = false;
	public static bool swipedUp = false;
	public static bool swipedDown = false;
	
	
	public bool debugWithArrowKeys = true;
	private Vector2 _swipe;

	Vector2 startPos;

	private bool _isDetecting = false;
	//float startTime;

	public void Update()
	{
		swipedRight = false;
		swipedLeft = false;
		swipedUp = false;
		swipedDown = false;

		if(Input.touches.Length > 0)
		{
			Touch t = Input.GetTouch(0);
			if(t.phase == TouchPhase.Began && !_isDetecting)
			{
				startPos = new Vector2(t.position.x/(float)Screen.width, t.position.y/(float)Screen.width);
				_isDetecting = true;
				_swipe = new Vector2(0, 0);
			}
			else if(_isDetecting && _swipe.magnitude >= DETECT_SWIPE_DISTANCE)
			{
				if (Mathf.Abs (_swipe.x) > Mathf.Abs (_swipe.y)) { // Horizontal swipe
					if (_swipe.x > 0) {
						swipedRight = true;
					}
					else {
						swipedLeft = true;
					}
				}
				else { // Vertical swipe
					if (_swipe.y > 0) {
						swipedUp = true;
					}
					else {
						swipedDown = true;
					}
				}

				_isDetecting = false;
			}
			else if (_isDetecting)
			{
				Vector2 endPos = new Vector2(t.position.x/(float)Screen.width, t.position.y/(float)Screen.width);
				_swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);
			}
		}

		if (debugWithArrowKeys) {
			swipedDown = swipedDown || Input.GetKeyDown (KeyCode.DownArrow);
			swipedUp = swipedUp|| Input.GetKeyDown (KeyCode.UpArrow);
			swipedRight = swipedRight || Input.GetKeyDown (KeyCode.RightArrow);
			swipedLeft = swipedLeft || Input.GetKeyDown (KeyCode.LeftArrow);
		}
	}
}