using System;

public class StageNotStarted : IGameStage {
	public void OnEnter(StageManager manager) {
		manager.SetHitableStage(GameManager.Stages.GameNotStarted);
	}

	public void OnUpdate(StageManager manager) {
	}

	public IGameStage CheckTransitions(StageManager manager) {
		int currentShadowScore = manager.GetShadowScore();
		int stage1Threshold = manager.GetStage1Threshold();

		if (currentShadowScore > 0 && currentShadowScore <= stage1Threshold) {
			return new StageDynamicCars();
		}
		return this;
	}
}