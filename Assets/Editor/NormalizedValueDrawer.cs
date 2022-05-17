/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

public class NormalizedValueDrawer : OdinValueDrawer< FFStudio.NormalizedValue >
{
	protected override void DrawPropertyLayout( GUIContent label )
	{
		FFStudio.NormalizedValue normalizedValue = this.ValueEntry.SmartValue;
		
		normalizedValue.value = SirenixEditorFields.RangeFloatField( label, normalizedValue.value * 100.0f, 0.0f, 100.0f ) / 100.0f;

		this.ValueEntry.SmartValue = normalizedValue;
	}
}
