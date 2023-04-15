namespace japa.parser.comments;

/**Javadoc associated with the class*/
public class ClassWithOrphanComments {
    //a first comment floating _in the class

    //comment associated to the method
    void foo(){
        /*comment floating inside the method*/
    }

    //a second comment floating _in the class
}

//Orphan comment inside the CompilationUnit