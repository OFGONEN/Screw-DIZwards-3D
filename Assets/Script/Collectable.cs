/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class Collectable : MonoBehaviour
{
#region Fields
	[ SerializeField ] Pool_UIPopUpText pool_UIPopUpText;
    [ SerializeField ] SharedFloatNotifier notif_currency;
    [ SerializeField ] float collectable_index;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void OnCollect()
    {
		var value = CurrentLevelData.Instance.currentLevel_Shown * collectable_index;
		notif_currency.SharedValue += value;

		var entity = pool_UIPopUpText.GetEntity();

		entity.Spawn( transform.position + Vector3.forward * GameSettings.Instance.collectable_text_offset,
        GameSettings.Instance.collectable_text_prefix + value, 
        GameSettings.Instance.collectable_text_size, 
        GameSettings.Instance.collectable_text_color );
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
