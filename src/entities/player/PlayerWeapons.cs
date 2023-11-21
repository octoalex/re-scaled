using System;
using Godot;
using OctoAlex.ReScaled.Common;

namespace OctoAlex.ReScaled.Entities.Player; 

public partial class PlayerWeapons : Node3D {

	[Export] private AnimationPlayer _animation;

	[Export] private PackedScene[] _weapons;

	private IWeapon[] _wEffective;

	private Node3D[] _wNodes;

	private int _active;

	public override void _Ready ( ) {
		_wEffective = new IWeapon[_weapons.Length];
		_wNodes = new Node3D[_weapons.Length];
		for (int i = 0; i < _weapons.Length; i++) {
			Node child = _weapons[i].Instantiate();
			_wEffective[i] = child as IWeapon ?? throw new ArgumentException("Initial Weapon must implement IWeapon");
			_wNodes[i] = child as Node3D;
			AddChild(child);
		}
		
		Deploy(0);
	}

	public override void _Input ( InputEvent evt ) {
		if (!Input.IsActionJustPressed("fire") ||
			!_wEffective[_active].CanFire()) return;
		_wEffective[_active].Fire();
	}

	public void AddWeapon ( PackedScene weapon ) {
		Node child = weapon.Instantiate();
		IWeapon effective = child as IWeapon ?? throw new ArgumentException("New Weapon must implement IWeapon");
		AddChild(child);
		
		var bufferI = new IWeapon[_wEffective.Length + 1];
		Array.Copy(_wEffective, bufferI, _wEffective.Length);
		bufferI[_wEffective.Length] = effective;
		_wEffective = bufferI;

		var bufferNode = new Node3D[_wNodes.Length + 1];
		Array.Copy(_wNodes, bufferNode, _wNodes.Length);
		bufferNode[_wNodes.Length] = child as Node3D;
		_wNodes = bufferNode;
	}

	private void Deploy ( int next ) {
		_wNodes[_active].Visible = false;
		_wNodes[next].Visible = true;
		_active = next;
		_wEffective[next].Deploy();
	}
}
