namespace a.b.c;


public class ExampleClass {

    protected final DataObjectFactory doFactory;


    public ExampleClass() {
        doFactory = null;
    }


    public DataObject getDataObject(String objectID) {
        return doFactory.getDataObject(objectID);
    }

}
