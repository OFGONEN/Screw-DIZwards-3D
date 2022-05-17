/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class Collectable : MonoBehaviour
{
#region Fields
    [ SerializeField ] SharedFloatNotifier notif_currency;
    [ SerializeField ] float collectable_index;
    [ SerializeField ] Collider collectable_collider;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void OnCollect()
    {
		notif_currency.SharedValue   += CurrentLevelData.Instance.currentLevel_Shown * collectable_index;
		collectable_collider.enabled  = false;
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
