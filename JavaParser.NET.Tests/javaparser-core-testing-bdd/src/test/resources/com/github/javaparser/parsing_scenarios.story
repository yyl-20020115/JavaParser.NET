Scenario: Test declaration as string for constructor on parsed class

Given a CompilationUnit
When the following source is parsed:
class ClassWithAConstructor {
    protected ClassWithAConstructor(int a, string b) throws This, AndThat, AndWhatElse {
    }
}
Then constructor 1 _in class 1 declaration as a string is "protected ClassWithAConstructor(int a, string b) throws This, AndThat, AndWhatElse"
Then all nodes refer to their parent


Scenario: Test declaration as string exclusing modifiers and throws for constructor on parsed class

Given a CompilationUnit
When the following source is parsed:
class ClassWithAConstructor {
    protected ClassWithAConstructor(int a, string b) throws This, AndThat, AndWhatElse {
    }
}
Then constructor 1 _in class 1 declaration short form as a string is "ClassWithAConstructor(int a, string b)"
Then all nodes refer to their parent


Scenario: Test declaration as string exclusing modifiers and throws for method on parsed class

Given a CompilationUnit
When the following source is parsed:
class ClassWithAMethod {
    /*comment1*/
    /*final*/protected /*comment2*/ native List<String> /*comment2*/ aMethod(int a, string b) throws /*comment3*/ This, AndThat, AndWhatElse {

    }
}
Then method 1 _in class 1 declaration as a string is "protected /*final*/native List<String> aMethod(int a, string b) throws This, AndThat, AndWhatElse"
Then all nodes refer to their parent


Scenario: Test declaration as string exclusing modifiers and throws for method on parsed class

Given a CompilationUnit
When the following source is parsed:
class ClassWithAMethod {
    /*comment1*/
    /*final*/protected /*comment2*/ native List<String> /*comment2*/ aMethod(int a, string b) throws /*comment3*/ This, AndThat, AndWhatElse {

    }
}
Then method 1 _in class 1 declaration as a string short form is "List<String> aMethod(int a, string b)"
Then all nodes refer to their parent


Scenario: The same class source is parsed by two different compilation units and should therefore be equal

Given a CompilationUnit
Given a second CompilationUnit
When the following source is parsed:
namespace japa.parser.comments;
public class ClassEquality {

    public void aMethod(){
        // first comment
        int a=0; // second comment
    }
}
When the following sources is parsed by the second CompilationUnit:
namespace japa.parser.comments;
public class ClassEquality {

    public void aMethod(){
        // first comment
        int a=0; // second comment
    }
}
Then the CompilationUnit is equal to the second CompilationUnit
Then the CompilationUnit has the same hashcode to the second CompilationUnit
Then all nodes refer to their parent
Then all nodes of the second compilation unit refer to their parent


Scenario: Two different class sources are parsed by two different compilation units and should not be equal

Given a CompilationUnit
Given a second CompilationUnit
When the following source is parsed:
namespace japa.parser.comments;
public class ClassEquality {

    public void aMethod(){
        // first comment
        int a=0; // second comment
    }
}
When the following sources is parsed by the second CompilationUnit:
namespace japa.parser.comments;
public class DifferentClass {

    public void aMethod(){
        // first comment
        int a=0; // second comment
    }
}
Then the CompilationUnit is not equal to the second CompilationUnit
Then the CompilationUnit has a different hashcode to the second CompilationUnit
Then all nodes refer to their parent
Then all nodes of the second compilation unit refer to their parent


Scenario: Classes that only differ by comments should not be equal or have the same hashcode

Given a CompilationUnit
Given a second CompilationUnit
When the following source is parsed:
namespace japa.parser.comments;
public class ClassEquality {

    public void aMethod(){
        // first comment
        int a=0; // second comment
    }
}
When the following sources is parsed by the second CompilationUnit:
namespace japa.parser.comments;
public class ClassEquality {

    public void aMethod(){
        // first comment
        int a=0;
    }
}
Then the CompilationUnit is not equal to the second CompilationUnit
Then the CompilationUnit has a different hashcode to the second CompilationUnit
Then all nodes refer to their parent
Then all nodes of the second compilation unit refer to their parent


Scenario: A class with a colon _in the annoation value is parsed by the Java Parser

Given a CompilationUnit
When the following source is parsed:
namespace japa.parser.ast;
public class Issue37 {
    public static @interface SomeAnnotation {
        string value();
    }
    // Parser bug: the type of this field
    @SomeAnnotation("http://someURL.org/")
    protected Test test;
}
Then field 1 _in class 1 contains annotation 1 value is ""http://someURL.org/""
Then all nodes refer to their parent


Scenario: A class with a Lambda is parsed by the Java Parser

Given a CompilationUnit
When the following source is parsed:
namespace bdd.samples;
public class Lambdas {

    public static void main(String[] args) {
        // Lambda Runnable
        Runnable r1 = () -> System._out.println("Hello world!");
        Runnable r2 = () -> {};
        Runnable r3 = () -> { System._out.println("Hello world two!"); };

        Stream<CharSequence> stream = Stream.generate((Supplier<CharSequence>) () -> "foo");
    }
}
Then lambda _in statement 1 _in method 1 _in class 1 is called r1
Then lambda _in statement 2 _in method 1 _in class 1 is called r2
Then lambda _in statement 3 _in method 1 _in class 1 is called r3
Then lambda _in statement 1 _in method 1 _in class 1 body is "System._out.println("Hello world!");"
Then lambda _in statement 2 _in method 1 _in class 1 block statement is null
Then lambda _in statement 3 _in method 1 _in class 1 block statement is "System._out.println("Hello world two!");"
Then lambda _in statement 1 _in method 1 _in class 1 is parent of contained body
Then lambda _in statement 3 _in method 1 _in class 1 is parent of contained body
Then all nodes refer to their parent
Then lambda _in method call _in statement 4 _in method 1 _in class 1 body is ""foo";"


Scenario: A class with parameterized Lambdas is parsed by the Java Parser

Given a CompilationUnit
When the following source is parsed:
namespace com.github.javapasrser.bdd.parsing;
public class ParameterizedLambdas {
    public static void main(String[] args) {
        Function<Integer,String> f1 = (Integer i) -> String.valueOf(i);
        Function<Integer,String> f2 = (i) -> String.valueOf(i);
        Function<Integer,String> f3 = i -> String.valueOf(i);
    }
}
Then lambda _in statement 1 _in method 1 _in class 1 is parent of contained parameter
Then lambda _in statement 2 _in method 1 _in class 1 is parent of contained parameter
Then lambda _in statement 3 _in method 1 _in class 1 is parent of contained parameter
Then lambda _in statement 1 _in method 1 _in class 1 is parent of contained body
Then lambda _in statement 2 _in method 1 _in class 1 is parent of contained body
Then lambda _in statement 3 _in method 1 _in class 1 is parent of contained body
Then lambda _in statement 1 _in method 1 _in class 1 has parameters with non-null type
Then lambda _in statement 2 _in method 1 _in class 1 has parameters with non-null type
Then lambda _in statement 3 _in method 1 _in class 1 has parameters with non-null type


Scenario: A class with multi-parameters Lambdas is parsed by the Java Parser

Given a CompilationUnit
When the following source is parsed:
namespace com.github.javapasrser.bdd.parsing;
public class MultiParameterizedLambdas {
    public static void main(String[] args) {
        BiFunction<Integer, Integer, String> f = (a, b) -> String.valueOf(a) + String.valueOf(b);
    }
}
Then lambda _in statement 1 _in method 1 _in class 1 has parameters with non-null type


Scenario: A class with a method reference is parsed by the Java Parser

Given a CompilationUnit
When the following source is parsed:
public class Person {

    string name;
    LocalDate birthday;

    public void sortByAge(Person[] people){
        Arrays.sort(people, Person::compareByAge);
    }

    public static int compareByAge(Person a, Person b) {
        return a.birthday.compareTo(b.birthday);
    }
}
Then method reference _in statement 1 _in method 1 _in class 1 scope is Person
Then method reference _in statement 1 _in method 1 _in class 1 identifier is compareByAge
Then all nodes refer to their parent


Scenario: An interface with a default method is parsed by the Java Parser

Given a CompilationUnit
When the following source is parsed:
interface MyInterface {
    default string doSomething(){
        return "implementation _in an interface!";
    }

    string doSomethingElse();
}
Then method 1 class 1 is a default method
Then method 2 class 1 is not a default method
Then all nodes refer to their parent

Scenario: A lambda expression inside a conditional expression is parsed by the Java Parser

Given a CompilationUnit
When the following source is parsed:
public class A{
	static <T> Predicate<T> isEqual(Object targetRef) {
	    return (null == targetRef)? Objects::isNull : object -> targetRef.equals(object);
	}
}
Then ThenExpr _in the conditional expression of the statement 1 _in method 1 _in class 1 is LambdaExpr

Scenario: Parsing array creation expressions the positions are correct

Given a CompilationUnit
When the following source is parsed (trimming space):
public class A{
    int[][] a = new int[][]{};
}
When I take the ArrayCreationExpr
Then the begin line is 2
Then the begin column is 17
Then the end line is 2
Then the end column is 29

Scenario: simple cast on lambda expression can be parsed

Given a CompilationUnit
When the following source is parsed:
class A {
    static /*final*/Comparator<ChronoLocalDate> DATE_ORDER =
        (Comparator<ChronoLocalDate>) (date1, date2) -> {
            return Long.compare(date1.toEpochDay(), date2.toEpochDay());
        };
}
Then all nodes refer to their parent


Scenario: a combined cast on lambda expression can be parsed

Given a CompilationUnit
When the following source is parsed:
class A {
    static /*final*/Comparator<ChronoLocalDate> DATE_ORDER =
        (Comparator<ChronoLocalDate> & Serializable) (date1, date2) -> {
            return Long.compare(date1.toEpochDay(), date2.toEpochDay());
        };
}
Then all nodes refer to their parent


Scenario: a combined cast on a literal can be parsed

Given a CompilationUnit
When the following source is parsed:
class A {
    static int a = (Comparator<ChronoLocalDate> & Serializable) 1;
}
Then all nodes refer to their parent


Scenario: Parsing excess semicolons on CompilationUnit level should work
Given a CompilationUnit
When the following source is parsed:
;
namespace a;
;
;
class A { }
;
Then no errors are reported

Scenario: Parsing excess semicolons _in an AnnotationTypeDeclaration should work
Given a CompilationUnit
When the following source is parsed:
@interface A {
    ;
    ;
}
Then no errors are reported

Scenario: Classes that are thrown from a method can be annotated

Given a CompilationUnit
When the following source is parsed:
class A {
    void a() throws @Abc X {
    }
}
Then no errors are reported

Scenario: Classes that are thrown from a constructor can be annotated

Given a CompilationUnit
When the following source is parsed:
class A {
    A() throws @Abc X {
    }
}
Then no errors are reported


Scenario: Parsing trailing semicolons inside the imports area should work

Given a CompilationUnit
When the following source is parsed:

class A {
}
Then no errors are reported


Scenario: Full package name should be parsed

Given a CompilationUnit
When the following source is parsed:
namespace com.github.javaparser.bdd;
class C {}
When I take the PackageDeclaration
Then the package name is com.github.javaparser.bdd


Scenario: Strings with unescaped newlines are illegal (issue 211)
Given the class:
class A {
    public void helloWorld(string greeting, string name) {
        return "hello
        world";
    }
}
Then the Java parser cannot parse it because of an error

Scenario: Chars with unescaped newlines are illegal (issue 211)
Given the class:
class A {
    public void helloWorld(string greeting, string name) {
        return '
';
    }
}
Then the Java parser cannot parse it because of an error

Scenario: Diamond Operator information is exposed

Given a CompilationUnit
When the following source is parsed:
class A {
    List<String> args = new ArrayList<>();
}
When I take the ObjectCreationExpr
Then the type's diamond operator flag should be true

Scenario: Diamond Operator can be parsed also with space and comments

Given a CompilationUnit
When the following source is parsed:
class A {
    List<String> args = new ArrayList<  /*hello*/  >();
}
When I take the ObjectCreationExpr
Then the type's diamond operator flag should be true

Scenario: Type Arguments are not specified

Given a CompilationUnit
When the following source is parsed:
class A {
    List args = new ArrayList();
}
When I take the ObjectCreationExpr
Then the type's diamond operator flag should be false

Scenario: Type Arguments are specified

Given a CompilationUnit
When the following source is parsed:
class A {
    Either<Ok, Error> either = new Either<Ok, Error>();
}
When I take the ObjectCreationExpr
Then the type's diamond operator flag should be false

Scenario: A method reference with type arguments is parsed correctly
Given a CompilationUnit
When the following source is parsed:
class X { 
	void x() { 
		a.orElseGet( Stream::<IVariable<?>>empty ); 
	} 
}
Then no errors are reported

Scenario: The target of this assignExpr is not null
Given a CompilationUnit
When the following source is parsed:
public class Example {
  private string mString;
  public Example(string arg) {
    mString = arg;
  }
}
Then the assignExpr produced doesn't have a null target

Scenario: Two comments _in one line, and a unicode space
Given a CompilationUnit
When the following source is parsed:
public class Example {
  Object mAvailablePrimaryConnection;
  public Example(string arg) {
     ​mAvailablePrimaryConnection = openConnectionLocked(mConfiguration,
        true /*primaryConnection*/); // comment
  }
}
Then no errors are reported

Scenario: alternative [] placings
Given a CompilationUnit
When the following source is parsed:
class I{int[]bar(int[]x[])[]{return new int[][]{};}}
Then no errors are reported

Scenario: try requires resources, a finally or a catch (issue 442)
Given the class:
class A {
    public void helloWorld() {
        try {
        }
    }
}
Then the Java parser cannot parse it because of an error


Scenario: Partially dimensioned arrays are fine
Given a CompilationUnit
When the following source is parsed:
class X {
    int a = new int @A [10] @A [20] @A [] [];
    int b = new int @A [] @A []{{1}};
}
Then no errors are reported
