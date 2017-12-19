using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;
namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
	public class DynamicWander : DynamicSeek
	{
		public float TurnAngle { get; private set; }
		public override string Name {
			get { return "Wander"; }
		}
		
		public float WanderOrientation { get; set; }
		public float WanderOffset { get; set; }
		public float WanderRadius { get; set; }
		public float WanderRate { get; set; }

		public DynamicWander ()
		{
			this.Target = new KinematicData ();
		}


		public override MovementOutput GetMovement ()
		{
			WanderOrientation += RandomHelper.RandomBinomial () * WanderRate;


			float targetOrientation = WanderOrientation + this.Character.orientation;

			Vector3 circleCenter = this.Character.position + WanderOffset * this.Character.GetOrientationAsVector ();

			this.Target.position = circleCenter + WanderRadius * MathHelper.ConvertOrientationToVector (targetOrientation);

			var output = base.GetMovement ();

			output.wanderOffset = circleCenter;
			output.wanderRadius = WanderRadius;

			return output;
		}
	}
}
