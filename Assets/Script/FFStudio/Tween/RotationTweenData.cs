/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;

namespace FFStudio
{
    public class RotationTweenData : TweenData
    {
        public enum RotationMode { Local, World }
        
#region Fields
    [ Title( "Rotation Tween" ) ]
        [ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ), SuffixLabel( "Degrees (°)" ) ] public float deltaAngle;
        [ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ), SuffixLabel( "Degrees/Seconds (°/s)" ), Min( 0 ) ] public float angularSpeedInDegrees;
        [ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ) ] public RotationMode rotationMode;
        [ BoxGroup( "Tween" ), PropertyOrder( int.MinValue ), ValueDropdown( "VectorValues" ), LabelText( "Rotate Around" ) ]
            public Vector3 rotationAxisMaskVector = Vector3.right;

        float Duration => Mathf.Abs( deltaAngle / angularSpeedInDegrees );

        IEnumerable VectorValues = new ValueDropdownList< Vector3 >()
        {
            { "X",   Vector3.right      },
            { "Y",   Vector3.up         },
            { "Z",   Vector3.forward    }
        };
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
#endregion

#region Implementation
        protected override void CreateAndStartTween( UnityMessage onComplete, bool isReversed = false )
        {
			if( rotationMode == RotationMode.Local )
				recycledTween.Recycle( transform.DOLocalRotate( rotationAxisMaskVector * deltaAngle, Duration, RotateMode.LocalAxisAdd ), onComplete );
			else
				recycledTween.Recycle( transform.DORotate( rotationAxisMaskVector * deltaAngle, Duration, RotateMode.WorldAxisAdd ), onComplete );

			recycledTween.Tween // Don't need to set SetRelative() as RotateMode.XXXAxisAdd automatically means relative end value.
				 .SetEase( easing )
				 .SetLoops( loop ? -1 : 0, loopType );

#if UNITY_EDITOR
			recycledTween.Tween.SetId( "_ff_rotation_tween___" + description );
#endif

			base.CreateAndStartTween( onComplete, isReversed );
		}
#endregion

#region EditorOnly
#if UNITY_EDITOR
#endif
#endregion
    }
}
