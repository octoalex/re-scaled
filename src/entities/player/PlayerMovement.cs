using Godot;
using OctoAlex.ReScaled.Common;

namespace OctoAlex.ReScaled.Entities.Player;

public partial class PlayerMovement : CharacterBody3D {

	[Export] private float _walkSpeed = 5.0f;

	[Export] private float _acceleration = 10.0f;

	[Export] private float _sensitivity = 50f;

	[Export] private Camera3D _camera;

	private MouseInput _mouse;

	public override void _Ready ( ) {
		_mouse = new MouseInput(GetViewport());
	}

	public override void _PhysicsProcess ( double delta ) {
		Vector3 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector(
			"move_left", "move_right", 
			"move_forward", "move_backward"
			);
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero) {
			velocity.X = Mathf.Clamp(velocity.X + direction.X * _acceleration, -_walkSpeed, _walkSpeed);
			velocity.Z = Mathf.Clamp(velocity.Z + direction.Z * _acceleration, -_walkSpeed, _walkSpeed);
		} else {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, _acceleration);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, _acceleration);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Input ( InputEvent evt ) {
		bool hasMoved = _mouse.GetMouseRelative(evt, out Vector2 relative);
		if (!hasMoved) {
			return;
		}

		RotateY(-relative.X * _sensitivity);
		//_camera.RotateX(-relative.Y * _sensitivity);
		Vector3 camRotation = _camera.Rotation;
		camRotation.X = Mathf.Clamp(camRotation.X - relative.Y * _sensitivity, Mathf.DegToRad(-90), Mathf.DegToRad(90));
		_camera.Rotation = camRotation;
	}
}
