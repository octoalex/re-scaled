/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using Godot;
using System;
using OctoAlex.ReScaled.Common;
using OctoAlex.ReScaled.Entities.Bullet;

namespace OctoAlex.ReScaled.Weapons.Revolver;

public partial class Revolver : Node3D, IWeapon {

	[Export] private float _force;

	[Export] private Node3D _firePoint;
	
	[Export] private AnimationPlayer _animationPlayer;

	[Export] private PackedScene _bullet;
	
	private Node3D _bulletsParent;

	public IEntity Owner { get; set; }

	public bool CanFire ( ) {
		return !_animationPlayer.IsPlaying();
	}

	public void Fire ( ) {
		_animationPlayer.Play("fire");
		SpawnBullet(_bullet, _firePoint.GlobalPosition, _firePoint.GlobalRotation, _firePoint.GlobalTransform.Basis.Z, _force);
	}

	public void Deploy ( ) {
		_animationPlayer.Play("deploy");
	}

	private void SpawnBullet ( PackedScene bullet, Vector3 position, Vector3 rotation, Vector3 forwards, float force ) {
		var b = bullet.Instantiate();
		var bulletNode = b as Bullet ?? throw new Exception("Bullet must inherit from Bullet");
		GetTree().Root.AddChild(bulletNode);
		bulletNode.GlobalPosition = position;
		bulletNode.GlobalRotation = rotation;
		bulletNode.ApplyForce(forwards * force);
		bulletNode.SetInitialForwards(forwards);
		bulletNode.Owner = Owner;
		bulletNode.IgnoreOnCollideGroup = "player_bullet";
	}
}
