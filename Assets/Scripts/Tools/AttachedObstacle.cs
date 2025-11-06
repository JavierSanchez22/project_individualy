using UnityEngine;

public class AttachedObstacle : MonoBehaviour {
	[Tooltip("The minimum angle threshold which colliding with obstacles will result in gameover")]
	[SerializeField] private float _angleThreshold = 40f;

	private void OnCollisionEnter(Collision other) {
        // Esta línea reemplaza el código de colisión duplicado
		PlayerCollisionHelper.HandleObstacleCollision(other, _angleThreshold);
	}
}