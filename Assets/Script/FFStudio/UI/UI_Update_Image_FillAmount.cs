/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.UI;
using FFStudio;

public class UI_Update_Image_FillAmount : MonoBehaviour
{
#region Fields
	public SharedFloatNotifier sharedDataNotifier;

	public Image ui_Image;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnEnable()
    {
		sharedDataNotifier.Subscribe( OnSharedDataChange );
	}

    private void OnDisable()
    {
		sharedDataNotifier.Unsubscribe( OnSharedDataChange );
    }

    private void Awake()
    {
		OnSharedDataChange();
	}
#endregion

#region API
#endregion

#region Implementation
    private void OnSharedDataChange()
    {
		ui_Image.fillAmount = sharedDataNotifier.SharedValue;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
