using System;
using UnityEngine;

public class StageCarCrashAndCrystals : IGameStage {
	public void OnEnter(StageManager manager) {
		AudioManager.Instance.ChangeSoundWithFade(Sound.Type.BGM2, 1);
		manager.PrepareStageChange(GameManager.Stages.CarCrashAndCrystals, false);
	}

	public void OnUpdate(StageManager manager) {
	}

	public IGameStage CheckTransitions(StageManager manager) {
		int currentShadowScore = manager.GetShadowScore();
		int stage2Threshold = manager.GetStage2Threshold();
		int remakeThreshold = manager.GetRemakeStageThreshold();

		if (currentShadowScore > stage2Threshold && currentShadowScore <= remakeThreshold) {
			return new StageBridgeCollapse();
		}
		return this;
	}
}