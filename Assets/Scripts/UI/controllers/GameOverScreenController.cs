using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class GameOverScreenController : MonoBehaviour {
	
	[Header("Components")]
	[SerializeField] private GameObject _withNewBestScoreGroup;
	[SerializeField] private GameObject _belowBestScoreGroup;
	[SerializeField] private GameObject _continueGroup;
	[SerializeField] private GameObject _restartButton;
	[SerializeField] private TMP_Text _finalScoreText;
	[SerializeField] private TMP_Text _bestScoreText;
	[SerializeField] private GameObject _continueMenu; 

	private GameStateManager _gameStateManager;

	private void OnEnable() {
		GameEvents.OnGameOver += OnGameOver;
		GameEvents.OnUpdateFinalScore += UpdateFinalScore;
		GameEvents.OnUpdateBestScore += UpdateBestScore;
		GameEvents.OnContinue += HideGameOverMenu;
		GameEvents.OnPrepareContinue += () => ContinueHubVisibility(true);
		GameEvents.OnReplay += () => ContinueHubVisibility(false);
	}

	private void OnDisable() {
		GameEvents.OnGameOver -= OnGameOver;
		GameEvents.OnUpdateFinalScore -= UpdateFinalScore;
		GameEvents.OnUpdateBestScore -= UpdateBestScore;
		GameEvents.OnContinue -= HideGameOverMenu;
		GameEvents.OnPrepareContinue -= () => ContinueHubVisibility(true);
		GameEvents.OnReplay -= () => ContinueHubVisibility(false);
	}

	private void Start() {
		_gameStateManager = FindObjectOfType<GameStateManager>();
	}

	private void OnGameOver(bool isThereNewBestScore) {
		if (_gameStateManager == null) return;

		float willThereBeAContinue = Random.Range(0f, 1f);
		if (_gameStateManager.isFirstLose || willThereBeAContinue < _gameStateManager.continueChance)
			ChangeContinueGroupVisibility(true);
		else
			ChangeContinueGroupVisibility(false);

		ShowScoreGroup(isThereNewBestScore);
		_gameStateManager.isFirstLose = false;

		AudioManager.Instance.PauseAllTracks();
	}

	private void ChangeContinueGroupVisibility(bool visibility) {
		if (_restartButton != null)
			_restartButton.SetActive(!visibility);

		if (_continueGroup != null)
			_continueGroup.SetActive(visibility);
	}

	private void ShowScoreGroup(bool isThereNewBestScore) {
		if (_withNewBestScoreGroup != null)
			_withNewBestScoreGroup.SetActive(isThereNewBestScore);
		if (_belowBestScoreGroup != null)
			_belowBestScoreGroup.SetActive(!isThereNewBestScore);
		LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
	}

	private void UpdateFinalScore(int finalScore) {
		if (_finalScoreText != null)
			_finalScoreText.text = "You did: " + finalScore;
	}

	private void UpdateBestScore(int bestScore) {
		if (_bestScoreText != null)
			_bestScoreText.text = "Your best is: " + bestScore;
	}

	private void ContinueHubVisibility(bool visibility) {
		// Esto controla el menú "Waiting for Ad"
		if (_continueMenu != null)
			_continueMenu.SetActive(visibility);
	}

	private void HideGameOverMenu() {
		this.gameObject.SetActive(false);
	}
}