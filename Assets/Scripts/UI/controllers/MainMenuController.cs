using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour {
	public static event Action<bool> OnChangeSkin;

	[Header("Components")]
	[SerializeField] private TMP_Text _initBestScoreText;
	[SerializeField] private TMP_Text _initCoinsText;
	[SerializeField] private Button _initSettingsButton;
	[SerializeField] private Button _playButton;
	[SerializeField] private Button _arrowLeftButton;
	[SerializeField] private Button _arrowRightButton;
	[SerializeField] private Button _unlockButton;

	private GameStateManager _gameStateManager;

	private void OnEnable() {
		GameEvents.OnAssignSaveData += UpdateSavedPoints;
		GameEvents.OnUpdateBestScore += UpdateBestScore;
		GameEvents.OnUpdateCoins += UpdateCoins;
	}

	private void OnDisable() {
		GameEvents.OnAssignSaveData -= UpdateSavedPoints;
		GameEvents.OnUpdateBestScore -= UpdateBestScore;
		GameEvents.OnUpdateCoins -= UpdateCoins;
	}

	private void Start() {
		_gameStateManager = FindObjectOfType<GameStateManager>();
	}

	private void Update() {
		if (_gameStateManager == null) return;
		
		_playButton.interactable = SkinsSystem.isCurrentSkinUnlocked;
		HandleButtonsInteractibility(_gameStateManager.IsGamePaused);
	}

	public void ChangeSkinByLeft() => OnChangeSkin?.Invoke(false);
	public void ChangeSkinByRight() => OnChangeSkin?.Invoke(true);
	public void PlayButtonClick() => AudioManager.Instance.PlaySoundOneShot(Sound.Type.UIClick, 2);

	private void UpdateSavedPoints(SaveData data) {
		UpdateCoins(data.coins);
		UpdateBestScore(data.bestScore);
	}

	private void UpdateCoins(int coins) {
		if (_initCoinsText != null)
			_initCoinsText.text = coins.ToString();
	}

	private void UpdateBestScore(int bestScore) {
		if (_initBestScoreText != null)
			_initBestScoreText.text = bestScore.ToString();
	}
	
	private void HandleButtonsInteractibility(bool isPaused) {
		_initSettingsButton.interactable = !isPaused;
		_arrowLeftButton.interactable = !isPaused;
		_arrowRightButton.interactable = !isPaused;
		_unlockButton.interactable = !isPaused;
	}
}