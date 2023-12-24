/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using Godot;
using System;
using OctoAlex.ReScaled.Common;

namespace OctoAlex.ReScaled.Entities.Bullet;

public partial class Bullet : RigidBody3D {

	private Vector3 _prevPos;
	
	public Bullet ( ) {
		BodyEntered += _OnBulletCollide;
	}

	public void SetInitialForwards ( Vector3 forwards ) {
		_prevPos = GlobalPosition - forwards;
	}

	public override void _PhysicsProcess ( double delta ) {
		Vector3 direction = (GlobalPosition - _prevPos).Normalized();
		Vector3 cross = GlobalTransform.Basis.Z.Cross(direction).Normalized();
		float angle = GlobalTransform.Basis.Z.SignedAngleTo(direction, cross);
		Rotate(cross, angle);
		_prevPos = GlobalPosition;
	}

	private void _OnBulletCollide ( Node body ) {
		if (body.IsInGroup("entity") && body is IEntity bodyEntity) {
			bodyEntity.Hit();
		}
		QueueFree();
	}
}
