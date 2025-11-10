using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
using System.Collections;
using UnityEngine.UI;

public class GameOverScreenController : MonoBehaviour {
	
	[Header("Components")]
	[SerializeField] private GameObject _withNewBestScoreGroup;
	[SerializeField] private GameObject _belowBestScoreGroup;
	[SerializeField] private GameObject _continueGroup;
	[SerializeField] private Button _restartButton;
	[SerializeField] private TMP_Text _finalScoreText;
	[SerializeField] private GameObject _continueMenu;

    // --- ¡AQUÍ ESTÁ LA MODIFICACIÓN! ---
	[Header("Best Score Texts")]
	[Tooltip("El texto que dice 'New best Text' (dentro de With best Group)")]
	[SerializeField] private TMP_Text _newBestScoreText;
	[Tooltip("El texto que dice 'Best score Text' (dentro de Below best Group)")]
	[SerializeField] private TMP_Text _currentBestScoreText;

	private GameStateManager _gameStateManager;
	private ScoreManager _scoreManager; // Guardar referencia

	private void OnEnable() {
		// En el momento en que este panel se activa (OnEnable se llama
		// CADA VEZ que el GameObject se activa), buscamos los datos frescos.
		
		_gameStateManager = FindObjectOfType<GameStateManager>();
		_scoreManager = FindObjectOfType<ScoreManager>();

		// --- ¡AQUÍ ESTÁ LA LÓGICA CORREGIDA! ---
		if (_scoreManager != null) {
			// Actualizamos los textos de score INMEDIATAMENTE
			// (sin esperar un evento que podría no llegar)
			UpdateBestScore(_scoreManager.BestScore);
		}

		GameEvents.OnGameOver += HandleGameOverEvent; 
		GameEvents.OnUpdateFinalScore += UpdateFinalScore;
		GameEvents.OnContinue += HideGameOverMenu;
		GameEvents.OnPrepareContinue += () => ContinueHubVisibility(true);
		GameEvents.OnReplay += () => ContinueHubVisibility(false);
	}

	private void OnDisable() {
		GameEvents.OnGameOver -= HandleGameOverEvent;
		GameEvents.OnUpdateFinalScore -= UpdateFinalScore;
		GameEvents.OnContinue -= HideGameOverMenu;
		GameEvents.OnPrepareContinue -= () => ContinueHubVisibility(true);
		GameEvents.OnReplay -= () => ContinueHubVisibility(false);
	}

	private void Start() {
		_gameStateManager = FindObjectOfType<GameStateManager>();
        
		if (_restartButton != null && _gameStateManager != null) {
			_restartButton.onClick.RemoveAllListeners();
			_restartButton.onClick.AddListener(_gameStateManager.PlayAgain);
		}
	}

	private void HandleGameOverEvent(bool isThereNewBestScore) {
		if (_gameStateManager == null) return;

		float willThereBeAContinue = Random.Range(0f, 1f);
		if (_gameStateManager.isFirstLose || willThereBeAContinue < _gameStateManager.continueChance)
			ChangeContinueGroupVisibility(true);
		else
			ChangeContinueGroupVisibility(false);

		_gameStateManager.isFirstLose = false;
		AudioManager.Instance.PauseAllTracks();

		StartCoroutine(ShowScoreGroupRoutine(isThereNewBestScore));
	}

	private IEnumerator ShowScoreGroupRoutine(bool isThereNewBestScore) {
		RectTransform parentRect = GetComponent<RectTransform>();
		RectTransform childRectToRebuild = null;

		if (isThereNewBestScore) {
			if (_withNewBestScoreGroup != null) {
				_withNewBestScoreGroup.SetActive(true);
				childRectToRebuild = _withNewBestScoreGroup.GetComponent<RectTransform>();
			}
			if (_belowBestScoreGroup != null) {
				_belowBestScoreGroup.SetActive(false);
			}
		} else {
			if (_withNewBestScoreGroup != null) {
				_withNewBestScoreGroup.SetActive(false);
			}
			if (_belowBestScoreGroup != null) {
				_belowBestScoreGroup.SetActive(true);
				childRectToRebuild = _belowBestScoreGroup.GetComponent<RectTransform>();
			}
		}
		
		yield return null; 

		if (childRectToRebuild != null) {
			LayoutRebuilder.ForceRebuildLayoutImmediate(childRectToRebuild);
		}
		if (parentRect != null) {
			LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);
		}
	}

	private void ChangeContinueGroupVisibility(bool visibility) {
		if (_restartButton != null)
			_restartButton.gameObject.SetActive(!visibility);
		if (_continueGroup != null)
			_continueGroup.SetActive(visibility);
	}

	private void UpdateFinalScore(int finalScore) {
		if (_finalScoreText != null)
			_finalScoreText.text = "You did: " + finalScore;
	}

	private void UpdateBestScore(int bestScore) {
        // --- ¡MÉTODO MODIFICADO! ---
        // Este método ahora actualiza AMBOS campos de texto.
		if (_newBestScoreText != null)
			_newBestScoreText.text = "New Best: " + bestScore;
		if (_currentBestScoreText != null)
			_currentBestScoreText.text = "Your Best: " + bestScore;
	}

	private void ContinueHubVisibility(bool visibility) {
		if (_continueMenu != null)
			_continueMenu.SetActive(visibility);
	}

	private void HideGameOverMenu() {
		this.gameObject.SetActive(false);
	}
}