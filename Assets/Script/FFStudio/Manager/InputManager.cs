/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using Lean.Touch;
using Sirenix.OdinInspector;

namespace FFStudio
{
    public class InputManager : MonoBehaviour
    {
#region Fields (Inspector Interface)
	[ Title( "Fired Events" ) ]
		public SwipeInputEvent event_input_swipe;
		public ScreenPressEvent event_input_screenPress;
		public IntGameEvent event_input_tap;
		public GameEvent event_input_fingerDown;
		public GameEvent event_input_fingerUp;

	[ Title( "Shared Variables" ) ]
		public SharedReferenceNotifier notifier_reference_camera_main;
		public SharedVector2Notifier notif_input;
#endregion

#region Fields (Private)
		private int swipeThreshold;

		private Transform transform_camera_main;
		private Camera camera_main;
		private LeanTouch leanTouch;

		Vector2Delegate onFingerDown;
#endregion

#region Unity API
		private void OnEnable()
		{
			notifier_reference_camera_main.Subscribe( OnCameraReferenceChange );
		}

		private void OnDisable()
		{
			notifier_reference_camera_main.Unsubscribe( OnCameraReferenceChange );
		}

		private void Awake()
		{
			swipeThreshold = Screen.width * GameSettings.Instance.swipeThreshold / 100;

			leanTouch         = GetComponent< LeanTouch >();
			leanTouch.enabled = false;

			onFingerDown = OnFingerDown;
		}
#endregion
		
#region API
		public void Swiped( Vector2 delta )
		{
			event_input_swipe.ReceiveInput( delta );
		}
		
		public void Tapped( int count )
		{
			event_input_tap.eventValue = count;
			event_input_tap.Raise();
		}

		public void Lean_OnFingerUpdate( Vector2 vector )
		{
			onFingerDown( vector );
		}

		public void Lean_OnFingerUp()
		{
			onFingerDown = OnFingerDown;
			notif_input.SetValue_DontNotify( Vector2.zero );
			event_input_fingerUp.Raise();
		}
#endregion

#region Implementation
		private void OnCameraReferenceChange()
		{
			var value = notifier_reference_camera_main.SharedValue;

			if( value == null )
			{
				transform_camera_main = null;
				leanTouch.enabled = false;
			}
			else 
			{
				transform_camera_main = value as Transform;
				camera_main           = transform_camera_main.GetComponent< Camera >();
				leanTouch.enabled    = true;
			}
		}

		void OnFingerDown( Vector2 vector )
		{
			onFingerDown = OnFingerUpdate;

			event_input_fingerDown.Raise();
			notif_input.SetValue_NotifyAlways( vector );
		}

		void OnFingerUpdate( Vector2 vector )
		{
			notif_input.SetValue_NotifyAlways( vector );
		}
#endregion
    }
}