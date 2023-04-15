namespace issue1945.implementations;

public class HairTypeWool implements HairType<WoolRenderer> {
	
	@Override
	public WoolRenderer getRenderer() {
		return WoolRenderer.INSTANCE;
	}
	
}
