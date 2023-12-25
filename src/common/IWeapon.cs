/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

namespace OctoAlex.ReScaled.Common; 

public interface IWeapon {
	
	public enum AmmonitionsIdEnum {
		None,
		Bullet
	}
	
	public IEntity Owner { get; set; }
	
	public AmmonitionsIdEnum AmmonitionsID { get; }
	
	public int AmmonitionsAmount { get; }
	
	public bool CanFire ( );
	
	public void Fire ( );

	public void Deploy ( );
}
