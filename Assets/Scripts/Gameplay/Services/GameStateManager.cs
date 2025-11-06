using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour {
	public bool IsGameRunning { get; private set; }
	public bool IsGamePaused { get; private set; }
	public bool IsGamePlayable { get { return (IsGameRunning && !IsGamePaused); } }

	[Header("Transitions")]
	[SerializeField] private Animator _endTransitionAnimator;
	[SerializeField] private Animator _pauseAnimator;
	[SerializeField] private Animator _gameOverAnimator;
	[SerializeField] private float _endTransitionDuration = 1f;
	[SerializeField] private float _scaleTransitionDuration = .5f;

	[Header("Game Over Logic")]
	public bool isFirstLose = true;
	public float continueChance = .2f;

	private ScoreManager _scoreManager;

	private void Start() {
		_scoreManager = FindObjectOfType<ScoreManager>();
		UnfreezeTime();
	}

	public void Play() {
		AudioManager.Instance.PlaySound(Sound.Type.BGM1, 1);
		IsGameRunning = true;
        IsGamePaused = false; // Nos aseguramos de que no esté pausado
		GameEvents.InvokePlay();
		UnfreezeTime();
	}

	public void Replay() {
		AudioManager.Instance.ResumeTrack(1);
		IsGamePaused = false;
		GameEvents.InvokeReplay();
	}

	public void Pause() {
		AudioManager.Instance.PauseAllTracks();
		IsGamePaused = true;
		StartCoroutine(TransitionedPause());
	}

	public void Resume() {
		AudioManager.Instance.ResumeAllTracks();
		IsGamePaused = false;
		StartCoroutine(TransitionedResume());
	}

	public void GameOver(Sound.Type gameOverSound = Sound.Type.None) {
		AudioManager.Instance.PlaySoundOneShot(gameOverSound, 2);
		if (!IsGamePlayable)
			return;

		IsGameRunning = false;
		IsGamePaused = true;

		bool isNewBest = _scoreManager.CheckForNewBestScore();
		GameEvents.InvokeGameOver(isNewBest);
		GameEvents.InvokeUpdateFinalScore(_scoreManager.Score);

		SaveSystem.Save(_scoreManager.BestScore, FindObjectOfType<CurrencyManager>().Coins);
	}

	public void PlayAgain() {
        // --- INICIO DE LA CORRECCIÓN ---
        // Reseteamos el estado ANTES de recargar la escena.
        IsGameRunning = false;
        IsGamePaused = false;
        // --- FIN DE LA CORRECCIÓN ---

		UnfreezeTime();
		StartCoroutine(ReloadSceneAfterTransition());
	}

	public void PrepareContinue() => StartCoroutine(PrepareContinueAfterFade());

	public void Continue() => StartCoroutine(ContinueAfterFade());

	private IEnumerator PrepareContinueAfterFade() {
		yield return FadeTransition("out");
		GameEvents.InvokePrepareContinue();
	}

	private IEnumerator ContinueAfterFade() {
		AudioManager.Instance.ResumeTrack(2);
		IsGameRunning = true;
        IsGamePaused = false; // Nos aseguramos de que no esté pausado
		yield return FadeTransition("in");
		yield return TransitionedContinue();
	}

	private IEnumerator ReloadSceneAfterTransition() {
		_endTransitionAnimator.SetTrigger("FadeOut");
		yield return new WaitForSecondsRealtime(_endTransitionDuration);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public IEnumerator FadeTransition(string fadeType) {
		if (fadeType == "in") {
			yield return null;
			_endTransitionAnimator.SetTrigger("FadeIn");
		}
		else if (fadeType == "out")
			_endTransitionAnimator.SetTrigger("FadeOut");
		
		yield return new WaitForSecondsRealtime(_endTransitionDuration);
	}

	private IEnumerator TransitionedPause() {
		GameEvents.InvokePause();
		yield return WaitScaleUpTransition(_pauseAnimator);
		FreezeTime();
	}

	private IEnumerator TransitionedResume() {
		UnfreezeTime();
		yield return WaitScaleDownTransition(_pauseAnimator);
		GameEvents.InvokeResume();
	}

	private IEnumerator TransitionedContinue() {
		yield return WaitScaleDownTransition(_gameOverAnimator);
		GameEvents.InvokeContinue();
	}

	private IEnumerator WaitScaleUpTransition(Animator animator) {
		animator.SetTrigger("ScaleUpBouncy");
		yield return new WaitForSeconds(_scaleTransitionDuration);
	}

	private IEnumerator WaitScaleDownTransition(Animator animator) {
		animator.SetTrigger("ScaleDown");
		yield return new WaitForSeconds(_scaleTransitionDuration);
	}

	private void FreezeTime() => Time.timeScale = 0f;
	private void UnfreezeTime() => Time.timeScale = 1f;
}