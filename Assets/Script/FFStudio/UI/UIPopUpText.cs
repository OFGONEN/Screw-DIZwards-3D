/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;
using Sirenix.OdinInspector;

namespace FFStudio
{
	public class UIPopUpText : MonoBehaviour
	{
#region Fields
		[ SerializeField ] UnityEvent onSequenceComplete;
		[ SerializeField ] TextMeshProUGUI ui_text;

		RecycledSequence recycledSequence = new RecycledSequence();
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
		[ Button() ]
		public void Spawn( Vector3 position, string text, float size, Color color )
		{
			gameObject.SetActive( true );
			transform.position   = position;
			transform.localScale = Vector3.zero;

			var sequence = recycledSequence.Recycle( onSequenceComplete.Invoke );

			sequence.Append( transform.DOMoveY( transform.position.y + GameSettings.Instance.ui_PopUp_movement_delta, GameSettings.Instance.ui_PopUp_movement_duration ).SetEase( GameSettings.Instance.ui_PopUp_movement_ease ) );
			sequence.Join( transform.DOScale( size * Vector3.one, GameSettings.Instance.ui_PopUp_size_In_duration ).SetEase( GameSettings.Instance.ui_PopUp_size_In_ease ) );
			sequence.AppendInterval( GameSettings.Instance.ui_PopUp_size_wait );
			sequence.Append( transform.DOScale( Vector3.zero, GameSettings.Instance.ui_PopUp_size_Out_duration ).SetEase( GameSettings.Instance.ui_PopUp_size_Out_ease ) );

			ui_text.text  = text;
			ui_text.color = color;
		}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
	}
}