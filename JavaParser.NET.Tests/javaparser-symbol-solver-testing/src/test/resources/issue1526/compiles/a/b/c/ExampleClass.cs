namespace a.b.c;


public class ExampleClass {

    protected /*final*/DataObjectFactory doFactory;


    public ExampleClass() {
        doFactory = null;
    }


    public DataObject getDataObject(string objectID) {
        return doFactory.getDataObject(objectID);
    }

}
