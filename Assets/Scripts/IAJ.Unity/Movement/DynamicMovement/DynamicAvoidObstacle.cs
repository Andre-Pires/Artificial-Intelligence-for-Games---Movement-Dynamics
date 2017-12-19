using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
	public class DynamicAvoidObstacle : DynamicSeek
	{
		public override string Name {
			get { return "AvoidObstacle"; }
		}

		public float MaxLookAhead  { get; set; }
		public float AvoidMargin  { get; set; }
		public Collider collisionDetector { get; set; }
		public override KinematicData Target { get; set; }

		public DynamicAvoidObstacle (GameObject collider)
		{
			collisionDetector = collider.GetComponent<Collider> ();
			this.Target = new KinematicData ();
		}

		public override MovementOutput GetMovement ()
		{
			if (this.Character.velocity.x != 0 && this.Character.velocity.z != 0) {

				Ray rayVector = new Ray (this.Character.position, this.Character.velocity.normalized);
				RaycastHit hitInfo = new RaycastHit ();
				Debug.DrawRay (this.Character.position, this.Character.velocity.normalized * this.MaxLookAhead, Color.black);
				if (collisionDetector.Raycast (rayVector, out hitInfo, this.MaxLookAhead)) {
					Target.position = hitInfo.point + hitInfo.normal * AvoidMargin;
				} else {
					return new MovementOutput ();
				}
			}

			return base.GetMovement ();

		}
	}
}
