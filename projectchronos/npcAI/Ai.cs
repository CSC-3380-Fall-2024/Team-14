public abstract class AI {
	public AI () {
	}

	// MUST get time data VERY IMPORTANT
	public abstract void ExecuteAI(float delta);
}

public enum npcAI {
	DefaultAI, // will be adding more for actual behaviors
	MonteKillO,
	SisyphusAI
}
