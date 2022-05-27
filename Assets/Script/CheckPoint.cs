/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using TMPro;
using Shapes;
using Sirenix.OdinInspector;

public class CheckPoint : MonoBehaviour
{
#region Fields
 [ Title( "Setup" ) ]
    [ SerializeField ] public Color checkpoint_color;
    [ SerializeField ] public float checkpoint_text_duration;
    [ SerializeField ] public float checkpoint_text_height;
    [ SerializeField ] public float checkpoint_text_size;
    [ SerializeField ] public Ease checkpoint_text_ease;

  [ Title( "Components" ) ]
    [ SerializeField ] TextMeshProUGUI checkpoint_text_static;
    [ SerializeField ] TextMeshProUGUI checkpoint_text_dynamic;
    [ SerializeField ] Line checkpoint_line;

    RecycledSequence recycledSequence = new RecycledSequence();
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    [ Button() ]
    public void OnTrigger()
    {
		checkpoint_text_dynamic.color = Color.white;
		checkpoint_text_static.color  = Color.white;
		checkpoint_line.Color         = Color.white;

		var target = checkpoint_text_dynamic.transform;

		var sequence = recycledSequence.Recycle( OnComplete );
		sequence.Join( target.DOMoveY( checkpoint_text_height, checkpoint_text_duration )
			.SetRelative()
			.SetEase( checkpoint_text_ease ) );
		sequence.Join( target.DOScale( Vector3.one * checkpoint_text_size, checkpoint_text_duration )
			.SetEase( checkpoint_text_ease ) );
		sequence.Join( checkpoint_text_dynamic.DOColor( checkpoint_color, checkpoint_text_duration * 0.75f )
			.SetEase( checkpoint_text_ease ) );
		sequence.Join( DOTween.To( GetLineColor, SetLineColor, checkpoint_color, checkpoint_text_duration )
            .SetEase( Ease.Linear ) );
		sequence.Join( checkpoint_text_dynamic.DOFade( 0, checkpoint_text_duration * 0.25f )
			.SetEase( Ease.OutCubic )
			.SetDelay( checkpoint_text_duration * 0.75f ) );
	}
#endregion

#region Implementation
    Color GetLineColor()
    {
		return checkpoint_line.Color;
	}

    void SetLineColor( Color color )
    {
		checkpoint_line.Color = color;
	}

    void OnComplete()
    {
		checkpoint_text_dynamic.gameObject.SetActive( false );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}