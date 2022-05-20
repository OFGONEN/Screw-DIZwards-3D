/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace FFStudio
{
    public class LevelManager : MonoBehaviour
    {
#region Fields
      [ Title( "Shared Variables" ) ]
        public PlayerPrefsUtility playerPrefsUtility;

      [ Title( "Fired Events" ) ]
        public GameEvent levelFailedEvent;
        public GameEvent levelCompleted;

      [ Title( "Level Releated" ) ]
        public SharedFloatNotifier levelProgress;
        public SharedFloatNotifier level_currency;
#endregion

#region UnityAPI
        private void Awake()
        {
			level_currency.SharedValue = playerPrefsUtility.GetFloat( ExtensionMethods.Key_Currency, 0 );
		}
#endregion

#region API
        public void LevelLoadedResponse()
        {

			levelProgress.SetValue_NotifyAlways( 0 );

			var levelData = CurrentLevelData.Instance.levelData;

            // Set Active Scene.
			if( levelData.scene_overrideAsActiveScene )
				SceneManager.SetActiveScene( SceneManager.GetSceneAt( 1 ) );
            else
				SceneManager.SetActiveScene( SceneManager.GetSceneAt( 0 ) );
		}

        public void LevelRevealedResponse()
        {
		}

        public void LevelStartedResponse()
        {
        }

        public void LevelFinishedResponse()
        {
			playerPrefsUtility.SetFloat( ExtensionMethods.Key_Currency, level_currency.sharedValue );
		}
#endregion

#region Implementation
#endregion
    }
}