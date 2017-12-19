namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicStraightAhead : DynamicMovement
    {
        public override string Name
        {
            get { return "StraightAhead"; }
        }

        public override KinematicData Target { get; set; }

        public override MovementOutput GetMovement()
        {
            var output = new MovementOutput();

            output.linear = this.Character.GetOrientationAsVector();

            if (output.linear.sqrMagnitude > 0)
            {
                output.linear.Normalize();
                output.linear *= this.MaxAcceleration;
            }

            return output;
        }
    }
}
