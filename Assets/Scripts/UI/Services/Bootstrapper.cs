using UnityEngine;

public class Bootstrapper : MonoBehaviour {
	
	public static Bootstrapper Instance { get; private set; }

	private void Awake() {
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		
		Instance = this;
	
		DontDestroyOnLoad(gameObject);

		SaveSystem.LoadAllData();
	}
}