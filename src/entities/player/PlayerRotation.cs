/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using Godot;
using OctoAlex.ReScaled.Common;

namespace OctoAlex.ReScaled.Entities.Player;

public partial class PlayerRotation : Camera3D {

	[Export] private CharacterBody3D _player;

	[Export] private float _sensitivity = 15f;

	[Export] private Camera3D _camera;

	private MouseInput _mouse;
	
	// Called when the node enters the scene tree for the first time.

	public override void _Ready ( ) {
		_mouse = new MouseInput(GetViewport());
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Input ( InputEvent evt ) {
		bool hasMoved = _mouse.GetMouseRelative(evt, out Vector2 relative);
		if (!hasMoved) {
			return;
		}

		_player.RotateY(-relative.X * _sensitivity);
		Vector3 camRotation = _camera.Rotation;
		camRotation.X = Mathf.Clamp(camRotation.X - relative.Y * _sensitivity, Mathf.DegToRad(-90), Mathf.DegToRad(90));
		_camera.Rotation = camRotation;
	}
}
