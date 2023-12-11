using Godot;
using System;
using OctoAlex.ReScaled.Common;

namespace OctoAlex.ReScaled.Entities.Bullet;

public partial class Bullet : Node3D {
	private void _OnBulletCollide ( Node body ) {
		if (body.IsInGroup("entity") && body is IEntity bodyEntity) {
			bodyEntity.Hit();
		}
	}
}
