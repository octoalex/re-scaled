using Godot;
using System;
using OctoAlex.ReScaled.Common;

namespace OctoAlex.ReScaled.Entities.Bullet;

public partial class Bullet : RigidBody3D {

	public Bullet ( ) {
		BodyEntered += _OnBulletCollide;
	}

	private void _OnBulletCollide ( Node body ) {
		if (body.IsInGroup("entity") && body is IEntity bodyEntity) {
			bodyEntity.Hit();
		}
		QueueFree();
	}
}
