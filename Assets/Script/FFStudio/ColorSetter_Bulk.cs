/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

namespace FFStudio
{
	public class ColorSetter_Bulk : MonoBehaviour
	{
#region Fields
	[ Title( "Setup" ) ]
		
		[ SerializeField ] string propertyName;
		[ SerializeField ] Color color_toSet;
		[ SerializeField ] Color color_toReset;
		[ SerializeField ] bool startWithColorSet = true;

		int shaderID;

		Renderer[] renderers;
		MaterialPropertyBlock propertyBlock;
#endregion

#region Properties
#endregion

#region Unity API
		void Awake()
		{
			shaderID = Shader.PropertyToID( propertyName );

			renderers = GetComponentsInChildren< Renderer >();
			propertyBlock = new MaterialPropertyBlock();
		}
		
		void Start()
		{
			if( startWithColorSet )
				SetColor();
		}
#endregion

#region API
		public void SetColor( Color color )
		{
			this.color_toSet = color;

			SetColor();
		}

		[ Button ]
		public void SetColor()
		{
			for( int i = 0; i < renderers.Length; i++ )
			{
				var thisRenderer = renderers[ i ];
				thisRenderer.GetPropertyBlock( propertyBlock );
				propertyBlock.SetColor( shaderID, color_toSet );
				thisRenderer.SetPropertyBlock( propertyBlock );
            }
		}
		
		[ Button ]
		public void ResetColor()
		{
			for( int i = 0; i < renderers.Length; i++ )
			{
				var thisRenderer = renderers[ i ];
				thisRenderer.GetPropertyBlock( propertyBlock );
				propertyBlock.SetColor( shaderID, color_toReset );
				thisRenderer.SetPropertyBlock( propertyBlock );
            }
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