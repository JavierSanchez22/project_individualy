using System;
using UnityEngine;

public class UIManager : MonoBehaviour {
	
	[Header("Panel References")]
	[SerializeField] private GameObject _initMenu;
	[SerializeField] private GameObject _gameUI;
	[SerializeField] private GameObject _pauseMenu;
	[SerializeField] private GameObject _gameOverMenu;

	private GameStateManager _gameStateManager;

	private void OnEnable() {
		GameEvents.OnPlay += OnPlay;
		GameEvents.OnPause += OnPause;
		GameEvents.OnResume += OnResume;
		GameEvents.OnGameOver += isThereNewBestScore => OnGameOver();
	}

	private void OnDisable() {
		GameEvents.OnPlay -= OnPlay;
		GameEvents.OnPause -= OnPause;
		GameEvents.OnResume -= OnResume;
		GameEvents.OnGameOver -= isThereNewBestScore => OnGameOver();
	}

	private void Start() {
		_gameStateManager = FindObjectOfType<GameStateManager>();
		
		// Estado inicial al cargar la escena
		SetMenusVisibility(true, false, false);
		_gameUI.SetActive(false);
	}
	
	private void Update() {
		if (_gameStateManager != null) {
			_gameUI.SetActive(_gameStateManager.IsGameRunning);
		}
	}

	private void OnPlay() => SetMenusVisibility(false, false, false);

	private void OnPause() {
		SetMenusVisibility(false, true, false);
		if (_gameStateManager != null && !_gameStateManager.IsGameRunning)
			SetMenuVisibility(_initMenu, true);
	}

	private void OnResume() {
		if (_gameStateManager != null) {
			if (_gameStateManager.IsGamePaused) // Evita que se muestre el InitMenu después de un Game Over
				SetMenuVisibility(_initMenu, false);
			else if (!_gameStateManager.IsGameRunning)
				SetMenuVisibility(_initMenu, true);
		}
		SetMenuVisibility(_pauseMenu, false);
	}

	private void OnGameOver() {
		SetMenusVisibility(false, false, true);
	}

	private void SetMenusVisibility(bool mainVisibility, bool pauseVisibility, bool gameOverVisibility) {
		SetMenuVisibility(_initMenu, mainVisibility);
		SetMenuVisibility(_pauseMenu, pauseVisibility);
		SetMenuVisibility(_gameOverMenu, gameOverVisibility);
	}

	private void SetMenuVisibility(GameObject menu, bool visibility) {
		if (menu != null)
			menu.SetActive(visibility);
	}
}