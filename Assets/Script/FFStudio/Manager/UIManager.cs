/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Sirenix.OdinInspector;

namespace FFStudio
{
    public class UIManager : MonoBehaviour
    {
#region Fields
        [ Header( "Event Listeners" ) ]
        public EventListenerDelegateResponse levelLoadedResponse;
        public EventListenerDelegateResponse levelCompleteResponse;
        public EventListenerDelegateResponse levelFailResponse;
        public EventListenerDelegateResponse tapInputListener;

        [ Header( "UI Elements" ) ]
        public UI_Patrol_Scale level_loadingBar_Scale;
        public TextMeshProUGUI level_count_text_start;
        public TextMeshProUGUI level_count_text_end;
        public TextMeshProUGUI level_information_text;
        public TextMeshProUGUI level_tutorial_text;
        public UI_Patrol_Scale level_information_text_Scale;
        public Image loadingScreenImage;
        public Image foreGroundImage;
        public RectTransform tutorialObjects;
        public RectTransform tutorial_Hand;
        public RectTransform tutorial_text_target;
        public TextMeshProUGUI tutorial_text;

        [ Header( "Fired Events" ) ]
        public GameEvent levelRevealedEvent;
        public GameEvent levelStartedEvent;
        public GameEvent loadNewLevelEvent;
        public GameEvent resetLevelEvent;
        public ElephantLevelEvent elephantLevelEvent;

		RecycledTween recycledTween = new RecycledTween();
        RecycledSequence recycledSequence = new RecycledSequence(); // Tutorial Text

		bool tutorialOnCenter = false;

		// Delegates
		UnityMessage onTutorialCorrect;
        UnityMessage onTutorialWrong;
#endregion

#region Unity API
		private void OnEnable()
        {
            levelLoadedResponse.OnEnable();
            levelFailResponse.OnEnable();
            levelCompleteResponse.OnEnable();
            tapInputListener.OnEnable();
        }

        private void OnDisable()
        {
            levelLoadedResponse.OnDisable();
            levelFailResponse.OnDisable();
            levelCompleteResponse.OnDisable();
            tapInputListener.OnDisable();

			recycledTween.Kill();
		}

        private void Awake()
        {
			onTutorialCorrect = ExtensionMethods.EmptyMethod;
			onTutorialWrong   = ExtensionMethods.EmptyMethod;

			levelLoadedResponse.response   = LevelLoadedResponse;
            levelFailResponse.response     = LevelFailResponse;
            levelCompleteResponse.response = LevelCompleteResponse;
            tapInputListener.response      = ExtensionMethods.EmptyMethod;

			level_information_text.text = "Tap to Start";

			recycledTween.Recycle( tutorial_Hand.DOAnchorPosX( 0, 1f )
                .SetEase( Ease.OutCubic )
                .SetLoops( -1, LoopType.Restart ) );
		}
#endregion

#region TutorialText
        [ Button() ]
        public void SpawnTutorialText( string text )
        {
			tutorial_text.gameObject.SetActive( true );
			tutorial_text.fontSize = 144;
			tutorial_text.text     = text;
			tutorial_text.color    = Color.white;

			tutorialOnCenter = true;

			tutorial_text.rectTransform.anchoredPosition3D = Vector3.zero;

			var sequence = recycledSequence.Recycle( OnSpawnTutorialComplete );
			sequence.Append( tutorial_text.DOFontSize( 108, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) );
		}

        void OnSpawnTutorialComplete()
        {
			onTutorialCorrect = OnTutorialCorrect;
			onTutorialWrong   = OnTutorialWrong;
		}

        [ Button() ]
        void OnTutorialCorrect()
        {
            if( recycledSequence.IsPlaying() ) return;

            if( tutorialOnCenter )
            {
			    tutorial_text.color = Color.green;

				var firstSequence = recycledSequence.Recycle();
				firstSequence.Append( tutorial_text.DOColor( Color.white, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) );

				firstSequence.AppendInterval( 0.5f );
				firstSequence.Append( tutorial_text.rectTransform.DOMove( tutorial_text_target.position, GameSettings.Instance.ui_Entity_Move_TweenDuration ) );
				firstSequence.Join( tutorial_text.DOFontSize( 72, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) );

				tutorialOnCenter = false;
				return;
			}

			tutorial_text.color = Color.green;

			var sequence = recycledSequence.Recycle();
			sequence.Append( tutorial_text.DOColor( Color.white, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) );
        }

        [ Button() ]
        void OnTutorialWrong()
        {
            if( recycledSequence.IsPlaying() ) return;

			tutorial_text.color = Color.red;

			var sequence = recycledSequence.Recycle();
			sequence.Append( tutorial_text.rectTransform.DOShakeRotation(
				GameSettings.Instance.ui_Entity_Scale_TweenDuration
			) )
				.Join( tutorial_text.DOColor( Color.white, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) );
		}
#endregion

#region Implementation
        private void LevelLoadedResponse()
        {
			var sequence = DOTween.Sequence()
								.Append( level_loadingBar_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
								.Append( loadingScreenImage.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
								.AppendCallback( () => tapInputListener.response = StartLevel );

			level_count_text_start.text = CurrentLevelData.Instance.currentLevel_Shown.ToString();
			level_count_text_end.text   = ( CurrentLevelData.Instance.currentLevel_Shown + 1 ).ToString();
			level_tutorial_text.text    = CurrentLevelData.Instance.levelData.tutorial_text;

			levelLoadedResponse.response = NewLevelLoaded;
        }

        private void NewLevelLoaded()
        {
			level_count_text_start.text = CurrentLevelData.Instance.currentLevel_Shown.ToString();
			level_count_text_end.text   = ( CurrentLevelData.Instance.currentLevel_Shown + 1 ).ToString();

			level_information_text.text = "Tap to Start";

			var sequence = DOTween.Sequence();

			// Tween tween = null;

			sequence.Append( foreGroundImage.DOFade( 0.5f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
					// .Append( tween ) // TODO: UIElements tween.
					.Append( level_information_text_Scale.DoScale_Start( GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
					.AppendCallback( () => tapInputListener.response = StartLevel );

            // elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
            // elephantLevelEvent.elephantEventType = ElephantEvent.LevelStarted;
            // elephantLevelEvent.Raise();
        }

        private void LevelCompleteResponse()
        {
            var sequence = DOTween.Sequence();

			// Tween tween = null;

			level_information_text.text = "Completed\n\n<size=75%>Tap to Continue";

			sequence.Append( foreGroundImage.DOFade( 0.5f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
					// .Append( tween ) // TODO: UIElements tween.
					.Append( level_information_text_Scale.DoScale_Start( GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
					.AppendCallback( () => tapInputListener.response = LoadNewLevel );

			OnLevelFinished();

			elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
            elephantLevelEvent.elephantEventType = ElephantEvent.LevelCompleted;
            elephantLevelEvent.Raise();
        }

        private void LevelFailResponse()
        {
            var sequence = DOTween.Sequence();

			// Tween tween = null;
			string progress = ( Mathf.CeilToInt( GameSettings.Instance.notif_level_progress.sharedValue * 100 ) ).ToString();
			level_information_text.text = $"Level Failed\n\n<size=60%>{GameSettings.Instance.ReturnEndLevelText()} (%{progress})\n\n<size=80%>Tap to Continue";

			sequence.Append( foreGroundImage.DOFade( 0.5f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
                    // .Append( tween ) // TODO: UIElements tween.
					.Append( level_information_text_Scale.DoScale_Start( GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
					.AppendCallback( () => tapInputListener.response = Resetlevel );

			OnLevelFinished();

			elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
            elephantLevelEvent.elephantEventType = ElephantEvent.LevelFailed;
            elephantLevelEvent.Raise();
        }

		private void StartLevel()
		{
			foreGroundImage.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration );

			level_information_text_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration );
			level_information_text_Scale.Subscribe_OnComplete( OnLevelRevealed );

			recycledTween.Kill();
			tutorialObjects.gameObject.SetActive( false );

			tapInputListener.response = ExtensionMethods.EmptyMethod;

			elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
			elephantLevelEvent.elephantEventType = ElephantEvent.LevelStarted;
			elephantLevelEvent.Raise();
		}

		private void LoadNewLevel()
		{
			tapInputListener.response = ExtensionMethods.EmptyMethod;

			var sequence = DOTween.Sequence();

			sequence.Append( foreGroundImage.DOFade( 1f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
			        .Join( level_information_text_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
			        .AppendCallback( loadNewLevelEvent.Raise );
		}

		private void Resetlevel()
		{
			tapInputListener.response = ExtensionMethods.EmptyMethod;

			var sequence = DOTween.Sequence();

			sequence.Append( foreGroundImage.DOFade( 1f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
			        .Join( level_information_text_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
			        .AppendCallback( resetLevelEvent.Raise );
		}

        void OnLevelRevealed()
        {
			levelRevealedEvent.Raise();
			levelStartedEvent.Raise();
		}

        void OnLevelFinished()
        {
			recycledSequence.Kill();
			tutorial_text.gameObject.SetActive( false );

			onTutorialCorrect = ExtensionMethods.EmptyMethod;
			onTutorialWrong   = ExtensionMethods.EmptyMethod;
		}
#endregion
    }
}