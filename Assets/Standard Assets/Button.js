//////////////////////////////////////////////////////////////
// Button.js
// Penelope iPhone Tutorial
//
// Button creates a tappable button (via GUITexture) that 
// handles touch input, taps, and phases.
//////////////////////////////////////////////////////////////

#pragma strict

@script RequireComponent( GUITexture )

static private var joysticks : Joystick[];					// A static collection of all joysticks
static private var enumeratedJoysticks : boolean = false;
static private var tapTimeDelta : float = 0.3;				// Time allowed between taps

var touchZone : Rect;
var isPressed : boolean;
var tapCount : int;											// Current tap count

private var lastFingerId = -1;								// Finger last used for this joystick
private var tapTimeWindow : float;							// How much time there is left for a tap to occur
private var fingerDownPos : Vector2;
private var fingerDownTime : float;
private var firstDeltaTime : float = 0.5;

private var gui : GUITexture;								// Joystick graphic
private var defaultRect : Rect;								// Default position / extents of the joystick graphic
private var guiTouchOffset : Vector2;						// Offset to apply to touch input
private var guiCenter : Vector2;							// Center of joystick

function Start()
{
	// Cache this component at startup instead of looking up every frame	
	gui = GetComponent( GUITexture );
	
	// Store the default rect for the gui, so we can snap back to it
	defaultRect = gui.pixelInset;	
    
    defaultRect.x += transform.position.x * Screen.width;// + gui.pixelInset.x; // -  Screen.width * 0.5;
    defaultRect.y += transform.position.y * Screen.height;// - Screen.height * 0.5;
    
    transform.position.x = 0.0;
    transform.position.y = 0.0;
		
	// This is an offset for touch input to match with the top left
	// corner of the GUI
	guiTouchOffset.x = defaultRect.width * 0.5;
	guiTouchOffset.y = defaultRect.height * 0.5;
	
	// Cache the center of the GUI, since it doesn't change
	guiCenter.x = defaultRect.x + guiTouchOffset.x;
	guiCenter.y = defaultRect.y + guiTouchOffset.y;
}

function Disable()
{
	gameObject.SetActive(false);
	enumeratedJoysticks = false;
}

function ResetJoystick()
{
	// Release the finger control and set the joystick back to the default position
	gui.pixelInset = defaultRect;
	lastFingerId = -1;
	isPressed = false;
	fingerDownPos = Vector2.zero;
}

function IsFingerDown() : boolean
{
	return (lastFingerId != -1);
}
	
function LatchedFinger( fingerId : int )
{
	// If another joystick has latched this finger, then we must release it
	if ( lastFingerId == fingerId )
		ResetJoystick();
}

function Update()
{	
	if ( !enumeratedJoysticks )
	{
		// Collect all joysticks in the game, so we can relay finger latching messages
		joysticks = FindObjectsOfType( Joystick ) as Joystick[];
		enumeratedJoysticks = true;
	}	
		
	var count = Input.touchCount;
	
	// Adjust the tap time window while it still available
	if ( tapTimeWindow > 0 )
		tapTimeWindow -= Time.deltaTime;
	else
		tapCount = 0;
	
	if ( count == 0 )
		ResetJoystick();
	else
	{
		for(var i : int = 0;i < count; i++)
		{
			var touch : Touch = Input.GetTouch(i);			
	
			var shouldLatchFinger = false;
			
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
				for ( var j : Joystick in joysticks )
				{
					if ( j != this )
						j.LatchedFinger( touch.fingerId );
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
					ResetJoystick();					
			}			
		}
	}
}