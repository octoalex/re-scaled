/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using Godot;
using OctoAlex.ReScaled.Common;

namespace OctoAlex.ReScaled.Weapons.TestWeapon;

public partial class TestWeapon : Node3D, IWeapon {

	[Export] private RayCast3D _ray;

	[Export] private AnimationPlayer _animation;

	[Export] private ulong _fireDelay;

	private ulong _lastFired;
	
	public void Fire ( ) {
		if (!_ray.IsColliding()) return;
		var collider = _ray.GetCollider() as CollisionObject3D;
		GD.Print(collider.GetParent().Name);
		_lastFired = Time.GetTicksMsec();
		_animation.Play("fire");
	}

	public void Deploy ( ) {
		_animation.Play("gun_spin");
	}

	public bool CanFire ( ) => !_animation.IsPlaying() && _lastFired + _fireDelay < Time.GetTicksMsec();
}
