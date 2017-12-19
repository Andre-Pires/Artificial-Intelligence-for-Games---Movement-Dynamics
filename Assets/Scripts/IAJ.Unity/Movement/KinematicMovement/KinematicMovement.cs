namespace Assets.Scripts.IAJ.Unity.Movement.KinematicMovement
{
    public abstract class KinematicMovement : Movement
    {
        public StaticData Character { get; set; }
        public float MaxSpeed { get; set; }
    }
}
