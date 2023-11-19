/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using Godot;
using OctoAlex.ReScaled.Common;

namespace OctoAlex.ReScaled.Entities.Player;

public partial class PlayerMovement : CharacterBody3D {

	[Export] private float _walkSpeed = 7f;

	[Export] private float _acceleration = 10f;

	[Export] private float _sensitivity = 15f;

	[Export] private ulong _msecRunInterval = 1000;

	[Export] private float _runSpeed = 12f;

	[Export] private float _mass = 3f;

	[Export] private float _jumpHeight = 1.25f;

	[Export] private float _airborneControlFactor = 0.35f;

	[Export] private float _slideDeceleration = 5f;

	[Export] private float _slideControl = 4f;

	[Export] private float _slideSpeed = 16f;

	[Export] private Camera3D _camera;

	[Export] private MeshInstance3D _mesh;

	[Export] private CollisionShape3D _collision;

	private MouseInput _mouse;

	private ulong _startPressed;

	private bool _pressed;

	private bool _runStarted;

	private bool _sliding;

	private float _slideVelocity;

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
		float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
		velocity.Y -= gravity * fDelta * _mass;

		if (IsOnFloor()) {
			if (Input.IsActionJustPressed("jump")) {
				velocity.Y = Mathf.Sqrt(2 * gravity * _jumpHeight * _mass);
			}
		}
			
		float speed = _walkSpeed;
		float accelerationFactor = 1;
		
		if (Input.IsActionJustPressed("slide")) {
			SlideBegin();
		} else if (Input.IsActionJustReleased("slide")) {
			SlideEnd();
		}
		
		if (!_sliding) {
			if (direction == Vector3.Zero) {
				_pressed = false;
				_runStarted = false;
			} else if (!_pressed) {
				_pressed = true;
				_startPressed = Time.GetTicksMsec();
			} else if ((_startPressed + _msecRunInterval < Time.GetTicksMsec() && IsOnFloor()) || _runStarted) {
				speed = _runSpeed;
				_runStarted = true;
			} else if (!IsOnFloor()) {
				accelerationFactor = _airborneControlFactor;
			}
			_velocityDirection = _velocityDirection.MoveToward(direction, _acceleration * fDelta * accelerationFactor);
		} else {
			if (_slideVelocity > 1 && (_velocityDirection * speed).Length() > 2f) {
				_velocityDirection = _velocityDirection.MoveToward(-Transform.Basis.Z, _slideControl * fDelta);
				speed = _slideVelocity;
				_slideVelocity = Mathf.Min(_slideVelocity - _slideDeceleration * fDelta, (_velocityDirection * speed).Length());
			} else {
				speed = 0;
			}
		}
		
		velocity.X = _velocityDirection.X * speed;
		velocity.Z = _velocityDirection.Z * speed;
		
		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Input ( InputEvent evt ) {
		bool hasMoved = _mouse.GetMouseRelative(evt, out Vector2 relative);
		if (!hasMoved) {
			return;
		}

		RotateY(-relative.X * _sensitivity);
		Vector3 camRotation = _camera.Rotation;
		camRotation.X = Mathf.Clamp(camRotation.X - relative.Y * _sensitivity, Mathf.DegToRad(-90), Mathf.DegToRad(90));
		_camera.Rotation = camRotation;
	}

	private void SlideBegin ( ) {
		var shape = _collision.Shape as CapsuleShape3D;
		shape.Height = 1f;
		var mesh = _mesh.Mesh as CapsuleMesh;
		mesh.Height = 1f;
		_sliding = true;
		// _camera.Position -= Vector3.Up;
		_slideVelocity = _slideSpeed;
	}

	private void SlideEnd ( ) {
		var shape = _collision.Shape as CapsuleShape3D;
		shape.Height = 2f;
		var mesh = _mesh.Mesh as CapsuleMesh;
		mesh.Height = 2f;
		_sliding = false;
		// _camera.Position += Vector3.Up;
	}
}
