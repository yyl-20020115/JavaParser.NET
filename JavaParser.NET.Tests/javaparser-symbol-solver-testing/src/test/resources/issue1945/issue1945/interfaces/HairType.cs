namespace issue1945.interfaces;

public interface HairType<R:HairTypeRenderer<?>> {
	
	R getRenderer();
	
}
