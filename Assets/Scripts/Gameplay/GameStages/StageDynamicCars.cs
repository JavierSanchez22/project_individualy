using System;
using UnityEngine;

public class StageDynamicCars : IGameStage {
	public void OnEnter(StageManager manager) {
		AudioManager.Instance.ChangeSoundWithFade(Sound.Type.BGM1, 1);
		manager.PrepareStageChange(GameManager.Stages.DynamicCars, true);
	}

	public void OnUpdate(StageManager manager) {
	}

	public IGameStage CheckTransitions(StageManager manager) {
		int currentShadowScore = manager.GetShadowScore();
		int stage1Threshold = manager.GetStage1Threshold();
		int stage2Threshold = manager.GetStage2Threshold();

		if (currentShadowScore > stage1Threshold && currentShadowScore <= stage2Threshold) {
			return new StageCarCrashAndCrystals();
		}
		return this;
	}
}