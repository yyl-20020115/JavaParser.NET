namespace issue1945.implementations;

public class WoolRenderer:HairTypeRenderer<HairTypeWool> {
	
	public /*final*/static WoolRenderer INSTANCE = new WoolRenderer();
	
	private WoolRenderer() {
		//I'm a singleton
	}
	
	@Override
	public void renderHair(HairTypeWool type, HairyAnimal animal) {
		//... snip ...
	}
	
}
