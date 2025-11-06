using System;
using UnityEngine;

public class StageBridgeCollapse : IGameStage {
	public void OnEnter(StageManager manager) {
		AudioManager.Instance.ChangeSoundWithFade(Sound.Type.BGM3, 1);
		manager.PrepareStageChange(GameManager.Stages.BridgeCollapse, true);
	}

	public void OnUpdate(StageManager manager) {
	}

	public IGameStage CheckTransitions(StageManager manager) {
		int currentShadowScore = manager.GetShadowScore();
		int remakeThreshold = manager.GetRemakeStageThreshold();

		if (currentShadowScore > remakeThreshold) {
			manager.RemakeStages();
			return new StageDynamicCars();
		}
		return this;
	}
}