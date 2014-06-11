using UnityEngine;
using System.Collections;

/**
 * Button.cs
 *
 * Mark Feaver
 * Based on Penelope iPhone Tutorial
 *
 * Button creates a tappable button (via GUITexture) that 
 * handles touch input, taps, and phases.
 */
[RequireComponent (typeof (GUITexture))]
public class Button : TouchControl {
	
	public bool isPressed;										// Is the button currently pressed
	
	private float fingerDownTime;
	
	// Use this for initialization
	void Start () {
		// Cache this component at startup instead of looking up every frame	
		gui = GetComponent<GUITexture>();
		
		// Store the default rect for the gui, so we can snap back to it
		defaultRect = gui.pixelInset;	
		
		defaultRect.x += transform.position.x * Screen.width;// + gui.pixelInset.x; // -  Screen.width * 0.5;
		defaultRect.y += transform.position.y * Screen.height;// - Screen.height * 0.5;
		
		transform.position = new Vector2(0.0f, 0.0f);
		
		// This is an offset for touch input to match with the top left
		// corner of the GUI
		guiTouchOffset.x = defaultRect.width * 0.5f;
		guiTouchOffset.y = defaultRect.height * 0.5f;
		
		// Cache the center of the GUI, since it doesn't change
		guiCenter.x = defaultRect.x + guiTouchOffset.x;
		guiCenter.y = defaultRect.y + guiTouchOffset.y;
	}
	
	protected override void Reset() {
		base.Reset();
		isPressed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if ( !enumeratedControls )
		{
			// Collect all joysticks in the game, so we can relay finger latching messages
			controls = FindObjectsOfType(typeof(Joystick)) as Joystick[];
			enumeratedControls = true;
		}	
		
		int count = Input.touchCount;
		
		// Adjust the tap time window while it still available
		if ( tapTimeWindow > 0 )
			tapTimeWindow -= Time.deltaTime;
		else
			tapCount = 0;
		
		if ( count == 0 )
			Reset();
		else
		{
			for(int i = 0;i < count; i++)
			{
				Touch touch = Input.GetTouch(i);			
				
				bool shouldLatchFinger = false;
				
				if ( gui.HitTest( touch.position ) )
				{
					shouldLatchFinger = true;
				}		
				
				// Latch the finger if this is a new touch
				if ( shouldLatchFinger && ( lastFingerId == -1 || lastFingerId != touch.fingerId ) )
				{				
					lastFingerId = touch.fingerId;
					
					// Accumulate taps if it is within the time window
					if ( tapTimeWindow > 0 )
						tapCount++;
					else
					{
						tapCount = 1;
						tapTimeWindow = tapTimeDelta;
					}
					
					// Tell other joysticks we've latched this finger
					foreach ( TouchControl control in controls )
					{
						if ( control != this )
							control.LatchedFinger( touch.fingerId );
					}						
				}				
				
				if ( lastFingerId == touch.fingerId )
				{	
					// Override the tap count with what the iPhone SDK reports if it is greater
					// This is a workaround, since the iPhone SDK does not currently track taps
					// for multiple touches
					if ( touch.tapCount > tapCount )
						tapCount = touch.tapCount;
					
					isPressed = true;
					
					if ( touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled )
						Reset();					
				}			
			}
		}
	}
}
