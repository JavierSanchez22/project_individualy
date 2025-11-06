using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour {
	
	[Header("Components")]
	[SerializeField] private Slider _bgmSlider;
	[SerializeField] private Slider _sfxSlider;
	[SerializeField] private GameObject _lowGraphicsSign;

	private bool _isPointerOnPauseMenu = false;
	private GameStateManager _gameStateManager;
	private GraphicsManager _graphicsManager;

	private void OnEnable() {
		GameEvents.OnUpdateVolume += UpdateVolumeSlider;
		GameEvents.OnChangedQuality += SelectPauseMenu;
	}

	private void OnDisable() {
		GameEvents.OnUpdateVolume -= UpdateVolumeSlider;
		GameEvents.OnChangedQuality -= SelectPauseMenu;
	}

	private void Start() {
		_gameStateManager = FindObjectOfType<GameStateManager>();
		_graphicsManager = FindObjectOfType<GraphicsManager>();
	}
	
	private void Update() {
		if (_graphicsManager != null)
			_lowGraphicsSign.SetActive(_graphicsManager.IsCurrentlyLowGraphics);
	}

	public void OnDeselectPauseMenu() {
		if (!_isPointerOnPauseMenu) {
			AudioManager.Instance.PlaySoundOneShot(Sound.Type.UIClick, 2);
			if (_gameStateManager != null)
				_gameStateManager.Resume();
		}
	}

	public void OnPointerEnterPauseMenu() => _isPointerOnPauseMenu = true;
	public void OnPointerExitPauseMenu() => _isPointerOnPauseMenu = false;
	public void SelectPauseMenu() => EventSystem.current.SetSelectedGameObject(this.gameObject);

	private void UpdateVolumeSlider(int track, float volume) {
		if (track == 1 && _bgmSlider != null)
			_bgmSlider.value = volume;
		else if (track == 2 && _sfxSlider != null)
			_sfxSlider.value = volume;
	}

	public void OnResumeButton() {
		if (_gameStateManager != null) {
			_gameStateManager.Resume();
		}
		
		if (_graphicsManager != null) {
			SaveSystem.SaveSettings(AudioManager.Instance.GetTrackVolume(1), AudioManager.Instance.GetTrackVolume(2), _graphicsManager.IsCurrentlyLowGraphics);
		}
	}
}