using System;

public interface IGameStage {
	void OnEnter(StageManager manager);
	void OnUpdate(StageManager manager);
	IGameStage CheckTransitions(StageManager manager);
}