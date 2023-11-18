/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using Godot;
using OctoAlex.ReScaled.Common;

namespace OctoAlex.ReScaled.Entities.Player;

public partial class PlayerMovement : CharacterBody3D {

	[Export] private float _walkSpeed = 5.0f;

	[Export] private float _acceleration = 10.0f;

	[Export] private float _sensitivity = 15f;

	[Export] private ulong _msecRunInterval = 1000;

	[Export] private float _runBoost = 1.5f;

	[Export] private Camera3D _camera;

	private MouseInput _mouse;

	private ulong _startPressed;

	private bool _pressed;

	private Vector3 _velocityDirection;

	public override void _Ready ( ) {
		_mouse = new MouseInput(GetViewport());
	}

	public override void _PhysicsProcess ( double delta ) {
		Vector3 velocity = Velocity;
		var fDelta = (float) delta;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector(
			"move_left", "move_right", 
			"move_forward", "move_backward"
			);
		
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		_velocityDirection = _velocityDirection.MoveToward(direction, _acceleration * fDelta);
		
		velocity.X = _velocityDirection.X * _walkSpeed;
		velocity.Z = _velocityDirection.Z * _walkSpeed;

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
