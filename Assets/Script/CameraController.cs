/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

public class CameraController : MonoBehaviour
{
#region Fields
	[ SerializeField ] SharedReferenceNotifier notif_nut_transform;

	// Private
	[ ShowInInspector, ReadOnly ] Transform target_transform;
	[ ShowInInspector, ReadOnly ] Transform child_transform;
	UnityMessage onUpdateMethod;
	Vector3 target_offset;

	RecycledTween recycledTween = new RecycledTween();
#endregion

#region Properties
#endregion

#region Unity API
	private void Awake()
	{
		onUpdateMethod = ExtensionMethods.EmptyMethod;

	}

	private void Start()
	{
		target_transform = notif_nut_transform.SharedValue as Transform;
		transform.position = target_transform.position + GameSettings.Instance.camera_offset_start;
	}

	private void OnDisable()
	{
		recycledTween.Kill();
		onUpdateMethod = ExtensionMethods.EmptyMethod;
	}

	private void Update()
	{
		onUpdateMethod();
	}
#endregion

#region API
	public void OnLevelStart()
	{
		child_transform  = transform.GetChild( 0 ); // Shake Transform
		onUpdateMethod   = FollowTargetWithOffset;

		recycledTween.Recycle( transform.DORotate( Vector3.right * GameSettings.Instance.camera_angle,
			GameSettings.Instance.camera_angle_duration ) );
	}

    // EditorCall
    public void OnCameraShake()
    {
		recycledTween.Recycle( child_transform.DOShakePosition( GameSettings.Instance.camera_shake_duration, GameSettings.Instance.camera_shake_strength ) );
	}
#endregion

#region Implementation
	void FollowTargetWithOffset()
	{
		var targetPosition = target_transform.position + GameSettings.Instance.camera_offset;
		transform.position = Vector3.Lerp( transform.position, targetPosition, GameSettings.Instance.camera_velocity * Time.deltaTime );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}