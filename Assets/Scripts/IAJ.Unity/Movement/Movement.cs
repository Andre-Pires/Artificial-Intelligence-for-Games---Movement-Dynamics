namespace Assets.Scripts.IAJ.Unity.Movement
{
    public abstract class Movement
    {
        public abstract string Name { get; }

        public abstract MovementOutput GetMovement();
    }
}
