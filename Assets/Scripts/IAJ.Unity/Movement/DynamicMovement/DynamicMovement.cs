using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public abstract class DynamicMovement
    {
        public abstract string Name { get; }
        public KinematicData Character { get; set; }
        public abstract KinematicData Target { get; set; }
        public float MaxAcceleration { get; set; }
        public Color MovementDebugColor { get; set; }

        public DynamicMovement()
        {
            this.MovementDebugColor = Color.black;
        }
        public abstract MovementOutput GetMovement();

    }
}
