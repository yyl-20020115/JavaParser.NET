namespace issue1945.implementations;

public class Sheep implements HairyAnimal {
	
	@Override
	public HairType<?> getHairType() {
		//simplified
		return new HairTypeWool();
	}
	
}
