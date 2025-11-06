using UnityEngine;

public class CurrencyManager : MonoBehaviour {
	public int Coins { get; private set; }
	private ScoreManager _scoreManager;

	private void Start() {
		_scoreManager = FindObjectOfType<ScoreManager>();
	}

	public void LoadCoins(int loadedCoins) {
		Coins = loadedCoins;
	}

	public void IncreaseCoin() {
		Coins++;
		GameEvents.InvokeUpdateCoins(Coins);
	}

	public void SpendCoins(int amount) {
		Coins -= amount;
		if (Coins < 0) {
			Coins = 0;
		}
		GameEvents.InvokeUpdateCoins(Coins);
		SaveSystem.Save(_scoreManager.BestScore, Coins);
	}
}