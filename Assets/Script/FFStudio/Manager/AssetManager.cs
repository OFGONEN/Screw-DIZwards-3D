/* Created by and for usage of FF Studios (2021). */
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace FFStudio
{
	/* This class holds references to ScriptableObject assets. These ScriptableObjects are singletons, so they need to load before a Scene does.
	 * Using this class ensures at least one script from a scene holds a reference to these important ScriptableObjects. */
	public class AssetManager : MonoBehaviour
	{
#region Fields
	[ Title( "UnityEvent" ) ]
	[ SerializeField ] UnityEvent onAwakeEvent;
	[ SerializeField ] UnityEvent onEnableEvent;
	[ SerializeField ] UnityEvent onStartEvent;

	[ Title( "Setup" ) ]
		[ SerializeField ] GameSettings gameSettings;
		[ SerializeField ] CurrentLevelData currentLevelData;

	[ Title( "Pool" ) ]
		[ SerializeField ] Pool_UIPopUpText pool_UIPopUpText;
		[ SerializeField ] ShatterPool[] pool_shatter;
#endregion

#region Implementation
		private void OnEnable()
		{
			onEnableEvent.Invoke();
		}

		private void Awake()
		{
			// Plugings
			Vibration.Init();


			pool_UIPopUpText.InitPool( transform, false );

			for( var i = 0; i < pool_shatter.Length; i++ )
			{
				pool_shatter[ i ].InitPool( transform, false );
			}
			
			onAwakeEvent.Invoke();
		}

		private void Start()
		{
			onStartEvent.Invoke();
		}
#endregion


#region API
		public void OnVibrate( VibrateEvent vibrateEvent )
		{
			switch ( vibrateEvent.vibrateMethod )
			{
				case VibrateMethod.Pop:
					Vibration.VibratePop();
					break;
				case VibrateMethod.Peek:
					Vibration.VibratePeek();
					break;
				case VibrateMethod.Nope:
					Vibration.VibrateNope();
					break;
				case VibrateMethod.Big:
					Vibration.Vibrate();
					break;
			}
		}
#endregion
	}
}