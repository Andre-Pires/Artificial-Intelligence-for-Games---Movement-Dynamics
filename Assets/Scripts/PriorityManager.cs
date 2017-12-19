using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.IAJ.Unity.Movement.Arbitration;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PriorityManager : MonoBehaviour
{
	public const float X_WORLD_SIZE = 55;
	public const float Z_WORLD_SIZE = 32.5f;
	public const float AVOID_MARGIN = 15.0f;
	public const float COLLISION_RADIUS = 5.0f;
	public const float MAX_SPEED = 20.0f;
	public const float MAX_ACCELERATION = 40.0f;
	public const float MAX_LOOK_AHEAD = 10.0f;
	public const float MAX_TIME_LOOK_AHEAD = 1.0f;
	public const float DRAG = 0.1f;
	public const float WANDER_RATE = 0.25f;

	private DynamicCharacter RedCharacter { get; set; }

	private Text RedMovementText { get; set; }

	private BlendedMovement Blended { get; set; }

	private PriorityMovement Priority { get; set; }

	private List<DynamicCharacter> Characters { get; set; }

	// Use this for initialization
	void Start ()
	{
		var textObj = GameObject.Find ("InstructionsText");
		if (textObj != null) {
			textObj.GetComponent<Text> ().text = 
				"Instructions\n\n" +
				"B - Blended\n" +
				"P - Priority\n" +
				"Q - stop"; 
		}

		this.RedMovementText = GameObject.Find ("RedMovement").GetComponent<Text> ();
		var redObj = GameObject.Find ("Red");

		this.RedCharacter = new DynamicCharacter (redObj)
	    {
	        Drag = DRAG,
	        MaxSpeed = MAX_SPEED
	    };
        
		var obstacles = GameObject.FindGameObjectsWithTag ("Obstacle");

		this.Characters = this.CloneSecondaryCharacters (redObj, 50, obstacles);
		this.Characters.Add (this.RedCharacter);

		this.InitializeMainCharacter (obstacles);

		//initialize all but the last character (because it was already initialized as the main character)
		foreach (var character in this.Characters.Take(this.Characters.Count-1)) {
			this.InitializeSecondaryCharacter (character, obstacles);
		}
	}

	private void InitializeMainCharacter (GameObject[] obstacles)
	{
		this.Priority = new PriorityMovement
        {
            Character = this.RedCharacter.KinematicData
        };

		this.Blended = new BlendedMovement
        {
            Character = this.RedCharacter.KinematicData
        };
        
		foreach (var obstacle in obstacles) {
			var avoidObstacleMovement = new DynamicAvoidObstacleWhiskers (obstacle)
			{
			    MaxAcceleration = MAX_ACCELERATION,
			    AvoidMargin = AVOID_MARGIN,
			    MaxLookAhead = MAX_LOOK_AHEAD,
			    Character = this.RedCharacter.KinematicData,
			    MovementDebugColor = Color.magenta
			};
			this.Blended.Movements.Add (new MovementWithWeight (avoidObstacleMovement, 5.0f));
			this.Priority.Movements.Add (avoidObstacleMovement);
		}

		foreach (var otherCharacter in this.Characters) {
			if (otherCharacter != this.RedCharacter) {
				var avoidCharacter = new DynamicAvoidCharacter (otherCharacter.KinematicData)
				{
				    Character = this.RedCharacter.KinematicData,
				    MaxAcceleration = MAX_ACCELERATION,
					CollisionRadius = COLLISION_RADIUS,
					MaxTimeLookAhead = MAX_TIME_LOOK_AHEAD,
					MovementDebugColor = Color.cyan 
				};

				this.Priority.Movements.Add (avoidCharacter);
			}
		}

		var wander = new DynamicWander
        {
			WanderOrientation = this.RedCharacter.KinematicData.orientation,
            Character = this.RedCharacter.KinematicData,
			WanderOffset = 20.0F,
			WanderRadius = 10.0F,
			WanderRate = WANDER_RATE,
			MaxAcceleration = MAX_ACCELERATION,
			MovementDebugColor = Color.yellow
        };

		this.Priority.Movements.Add (wander);
		this.Blended.Movements.Add (new MovementWithWeight (wander, obstacles.Length + this.Characters.Count));

		this.RedCharacter.Movement = this.Blended;

	}

	private void InitializeSecondaryCharacter (DynamicCharacter character, GameObject[] obstacles)
	{
		var priority = new PriorityMovement
        {
            Character = character.KinematicData
        };

		foreach (var obstacle in obstacles) {

			var avoidObstacleMovement = new DynamicAvoidObstacleWhiskers (obstacle)
			{
			    MaxAcceleration = MAX_ACCELERATION,
			    AvoidMargin = AVOID_MARGIN,
			    MaxLookAhead = MAX_LOOK_AHEAD,
			    Character = character.KinematicData,
			    MovementDebugColor = Color.magenta
			};
            
			priority.Movements.Add (avoidObstacleMovement);
		}

		foreach (var otherCharacter in this.Characters) {
			if (otherCharacter != character) {
				var avoidCharacter = new DynamicAvoidCharacter (otherCharacter.KinematicData)
				{
				    Character = character.KinematicData,
				    MaxAcceleration = MAX_ACCELERATION,
					CollisionRadius = COLLISION_RADIUS,
					MaxTimeLookAhead = MAX_TIME_LOOK_AHEAD,
				    MovementDebugColor = Color.cyan
				};

				priority.Movements.Add (avoidCharacter);
			}
		}

		var straightAhead = new DynamicStraightAhead
        {
            Character = character.KinematicData,
            MaxAcceleration = MAX_ACCELERATION,
            MovementDebugColor = Color.yellow
        };

		priority.Movements.Add (straightAhead);

		character.Movement = priority;
	}

	private List<DynamicCharacter> CloneSecondaryCharacters (GameObject objectToClone, int numberOfCharacters, GameObject[] obstacles)
	{
		var characters = new List<DynamicCharacter> ();
		for (int i = 0; i < numberOfCharacters; i++) {
			var clone = GameObject.Instantiate (objectToClone);
			//clone.transform.position = new Vector3(30,0,i*20);
			clone.transform.position = this.GenerateRandomClearPosition (obstacles);
			var character = new DynamicCharacter (clone)
            {
                MaxSpeed = MAX_SPEED,
                Drag = DRAG
            };
			//character.KinematicData.orientation = (float)Math.PI*i;
			characters.Add (character);
		}

		return characters;
	}


	private Vector3 GenerateRandomClearPosition (GameObject[] obstacles)
	{
		Vector3 position = new Vector3 ();
		var ok = false;
		while (!ok) {
			ok = true;

			position = new Vector3 (Random.Range (-X_WORLD_SIZE, X_WORLD_SIZE), 0, Random.Range (-Z_WORLD_SIZE, Z_WORLD_SIZE));

			foreach (var obstacle in obstacles) {
				var distance = (position - obstacle.transform.position).magnitude;

				//assuming obstacle is a sphere just to simplify the point selection
				if (distance < obstacle.transform.localScale.x + AVOID_MARGIN) {
					ok = false;
					break;
				}
			}
		}

		return position;
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Q)) {
			this.RedCharacter.Movement = null;
		} else if (Input.GetKeyDown (KeyCode.B)) {
			this.RedCharacter.Movement = this.Blended;
		} else if (Input.GetKeyDown (KeyCode.P)) {
			this.RedCharacter.Movement = this.Priority;
		}

		foreach (var character in this.Characters) {
			this.UpdateMovingGameObject (character);
		}

		this.UpdateMovementText ();
	}

	private void UpdateMovingGameObject (DynamicCharacter movingCharacter)
	{
		if (movingCharacter.Movement != null) {
			movingCharacter.Update ();
			movingCharacter.KinematicData.ApplyWorldLimit (X_WORLD_SIZE, Z_WORLD_SIZE);
			movingCharacter.GameObject.transform.position = movingCharacter.Movement.Character.position;
		}
	}

	private void UpdateMovementText ()
	{
		if (this.RedCharacter.Movement == null) {
			this.RedMovementText.text = "Red Movement: Stationary";
		} else {
			this.RedMovementText.text = "Red Movement: " + this.RedCharacter.Movement.Name;
		}
	}
}
