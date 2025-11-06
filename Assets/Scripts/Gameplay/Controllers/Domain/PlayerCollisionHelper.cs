using UnityEngine;

public static class PlayerCollisionHelper {

	public static void HandleObstacleCollision(Collision other, float angleThreshold) {
		if (other.gameObject.CompareTag("Player")) {
			for (int i = 0; i < other.contacts.Length; i++) {
				float currentAngleRefUp = Vector3.Angle(other.contacts[i].normal, Vector3.up);
				float currentAngleRefDown = Vector3.Angle(other.contacts[i].normal, Vector3.down);

				if (currentAngleRefUp <= angleThreshold || currentAngleRefDown <= angleThreshold) {
                    // --- INICIO DE LA CORRECCIÓN ---
                    // Buscamos PlayerMovement, no PlayerController
					PlayerMovement movement = other.gameObject.GetComponent<PlayerMovement>();
					if (movement != null) {
						movement.RechargeJumps();
					}
                    // --- FIN DE LA CORRECCIÓN ---
					break;
				}
				else
					Object.FindObjectOfType<GameStateManager>().GameOver(Sound.Type.Death);
			}
		}
	}
}