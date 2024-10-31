namespace ProjectChronos;

public partial class Sisyphus: BasicEnemy {
    public override void _Ready() {
        stats = new StatBlock(1000, 50, true, true, npcAI.DefaultAI, 1000);
        base._Ready();
    }
}