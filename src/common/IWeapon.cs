namespace OctoAlex.ReScaled.Common; 

public interface IWeapon {
	public bool CanFire ( );
	
	public void Fire ( );

	public void Deploy ( );
}
