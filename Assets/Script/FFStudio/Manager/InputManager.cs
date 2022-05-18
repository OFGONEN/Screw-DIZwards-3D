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

		Vector2Delegate onFingerUpdate;
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

			onFingerUpdate = ExtensionMethods.EmptyMethod;
			notif_input.SetValue_NotifyAlways( Vector2.zero );
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

		public void Lean_OnFingerDown()
		{
			event_input_fingerDown.Raise();
			onFingerUpdate = OnFingerUpdate;
		}

		public void Lean_OnFingerUpdate( Vector2 vector )
		{
			onFingerUpdate( vector );
		}

		public void Lean_OnFingerUp()
		{
			onFingerUpdate = ExtensionMethods.EmptyMethod;

			event_input_fingerUp.Raise();
			notif_input.SetValue_NotifyAlways( Vector2.zero );
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

		void OnFingerUpdate( Vector2 vector )
		{
			notif_input.SetValue_NotifyAlways( vector / Time.deltaTime / 60 );
		}
#endregion


// #if UNITY_EDITOR
// #region EditorOnly
		void OnGUI()
		{
			GUI.Label( new Rect( 25, 0, 200, 50 ), "FPS: " + 1f / Time.deltaTime );
			GUI.Label( new Rect( 25, 50, 200, 50 ), "Velocity Max: " + GameSettings.Instance.velocity_max );
			GUI.Label( new Rect( 25, 100, 200, 50 ), "Velocity Min: " + GameSettings.Instance.velocity_min );
			GUI.Label( new Rect( 25, 150, 200, 50 ), "Velocity Accelerate: " + GameSettings.Instance.velocity_accelerate );
			GUI.Label( new Rect( 25, 200, 200, 50 ), "Velocity Decelerate: " + GameSettings.Instance.velocity_decelerate );
			GUI.Label( new Rect( 25, 250, 200, 50 ), "Velocity Movement Cofactor: " + GameSettings.Instance.velocity_movement_cofactor );
			GUI.Label( new Rect( 25, 300, 200, 50 ), "Velocity Rotate Cofactor: " + GameSettings.Instance.velocity_rotate_cofactor );		
		}
// #endregion
// #endif
    }
}