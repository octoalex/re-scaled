using Godot;

namespace OctoAlex.ReScaled.Common;

public class MouseInput {

	private readonly Viewport _vp;
	
	public MouseInput ( Viewport vp ) {
		_vp = vp;
	}
	
	public bool GetMouseRelative ( InputEvent evt, out Vector2 relative ) {
		if (evt is not InputEventMouseMotion mmEvt) {
			relative = Vector2.Zero;
			return false;
		}

		relative = new Vector2(
			mmEvt.Relative.X / _vp.GetVisibleRect().Size.X, 
			mmEvt.Relative.Y / _vp.GetVisibleRect().Size.Y
			);
		return true;
	}
}
