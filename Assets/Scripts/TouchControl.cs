using UnityEngine;
using System.Collections;

public class TouchControl : MonoBehaviour {

	static protected TouchControl[] controls;					// A static collection of all joysticks
	static protected bool enumeratedControls = false;
	
	static protected float tapTimeDelta = 0.3f;					// Time allowed between taps
	public Rect touchZone;
	public int tapCount;										// Current tap count
	
	protected int lastFingerId = -1;							// Finger last used for this joystick
	protected float tapTimeWindow;								// How much time there is left for a tap to occur
	
	protected GUITexture gui;									// Joystick graphic
	protected Rect defaultRect;									// Default position / extents of the joystick graphic
	protected Vector2 guiTouchOffset;							// Offset to apply to touch input
	protected Vector2 guiCenter;								// Center of joystick
	
	public void Disable() {
		gameObject.SetActive(false);
		enumeratedControls = false;
	}
	
	protected virtual void Reset() {
		// Release the finger control and set the joystick back to the default position
		gui.pixelInset = defaultRect;
		lastFingerId = -1;
	}
	
	private bool IsFingerDown() {
		return (lastFingerId != -1);
	}
	
	public void LatchedFinger( int fingerId ) {
		// If another joystick has latched this finger, then we must release it
		if ( lastFingerId == fingerId )
			Reset();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
