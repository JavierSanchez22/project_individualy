using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
	// Eventos que los otros scripts (Movimiento, Visuales) escucharán
	public event Action OnJumpPressed;
	public event Action OnSwitchPressed;
	public event Action<bool> OnJumpHeld;

	private bool _isPaused = false;

	private void OnEnable() {
		// Escucha los inputs físicos
		InputsController.OnJump += HandleJump;
		InputsController.OnSwitch += HandleSwitch;
		InputsController.OnHoldingJump += HandleHold;

		// Escucha los eventos del juego
		GameEvents.OnPause += () => _isPaused = true;
		GameEvents.OnResume += () => _isPaused = false;
		GameEvents.OnGameOver += (isNewBest) => _isPaused = true;
		GameEvents.OnPlay += () => _isPaused = false;
		GameEvents.OnContinue += () => _isPaused = false;
	}

	private void OnDisable() {
		InputsController.OnJump -= HandleJump;
		InputsController.OnSwitch -= HandleSwitch;
		InputsController.OnHoldingJump -= HandleHold;
		
		GameEvents.OnPause -= () => _isPaused = true;
		GameEvents.OnResume -= () => _isPaused = false;
		GameEvents.OnGameOver -= (isNewBest) => _isPaused = true;
		GameEvents.OnPlay -= () => _isPaused = false;
		GameEvents.OnContinue -= () => _isPaused = false;
	}

	private void HandleJump() {
		if (_isPaused) return;
		OnJumpPressed?.Invoke();
	}

	private void HandleSwitch() {
		if (_isPaused) return;
		OnSwitchPressed?.Invoke();
	}

	private void HandleHold(bool isHolding) {
		if (_isPaused) return;
		OnJumpHeld?.Invoke(isHolding);
	}
}