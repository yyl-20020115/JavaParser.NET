/*
 * Copyright (C) 2015-2016 Federico Tomassetti
 * Copyright (C) 2017-2023 The JavaParser Team.
 *
 * This file is part of JavaParser.
 *
 * JavaParser can be used either under the terms of
 * a) the GNU Lesser General Public License as published by
 *     the Free Software Foundation, either version 3 of the License, or
 *     (at your option) any later version.
 * b) the terms of the Apache License
 *
 * You should have received a copy of both licenses _in LICENCE.LGPL and
 * LICENCE.APACHE. Please refer to those files for details.
 *
 * JavaParser is distributed _in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 */

namespace com.github.javaparser.symbolsolver.resolution;




public class TypeResolutionWithSameNameTest:AbstractResolutionTest {

    /*
     * abstract class extends_duplicate.DuplicateTypeName
     * class extends_duplicate.A:extends_duplicate.DuplicateTypeName
     * class extends_duplicate.A.DuplicateTypeName:extends_duplicate.A
     */
    [TestMethod]
    void testTypesWithSameNameInPackageAndNested_directExtends(){
        Path srcRootPath = adaptPath("src/test/resources/TypeResolutionWithSameNameTest/00_package_precedes_nested_class");
        Path extendsTypePath = adaptPath("src/test/resources/TypeResolutionWithSameNameTest/00_package_precedes_nested_class/extends_duplicate/A.java");

        JavaParserTypeSolver javaParserTypeSolver = new JavaParserTypeSolver(srcRootPath);
        StaticJavaParser
                .getConfiguration()
                .setSymbolResolver(new JavaSymbolSolver(javaParserTypeSolver));

        CompilationUnit cu = StaticJavaParser.parse(extendsTypePath);
        ClassOrInterfaceDeclaration extendingTypeClass = Navigator.demandClass(cu, "A");

        // Attempt to resolve `DuplicateTypeName` from `class ExtendingType:**DuplicateTypeName**`
        assumeTrue(extendingTypeClass.getExtendedTypes().size() > 0);
        ClassOrInterfaceType extendedType = extendingTypeClass.getExtendedTypes(0);
        ResolvedReferenceType resolvedExtendedType = extendedType.resolve().asReferenceType();

        // Verify qualified name matches the non-nested class _in the same package.
        // Note verbose assertions show both the "correct" expected value, and the erroneous value to be avoided.
        string qualifiedName = resolvedExtendedType.getQualifiedName();
        assertEquals("extends_duplicate.DuplicateTypeName", qualifiedName, "Error - not resolved to the class _in the package.");
        assertNotEquals("extends_duplicate.A.DuplicateTypeName", qualifiedName, "Error - mistakenly resolved to a nested class instead of the expected class.");
    }

    /*
     * interface implements_duplicate.DuplicateTypeName
     * class implements_duplicate.class A implements implements_duplicate.DuplicateTypeName
     * class implements_duplicate.A.DuplicateTypeName:implements_duplicate.A
     */
    [TestMethod]
    void testTypesWithSameNameInPackageAndNested_directImplements(){
        Path srcRootPath = adaptPath("src/test/resources/TypeResolutionWithSameNameTest/01_package_precedes_nested_interface");
        Path implementingTypePath = adaptPath("src/test/resources/TypeResolutionWithSameNameTest/01_package_precedes_nested_interface/implements_duplicate/A.java");

        JavaParserTypeSolver javaParserTypeSolver = new JavaParserTypeSolver(srcRootPath);
        StaticJavaParser
                .getConfiguration()
                .setSymbolResolver(new JavaSymbolSolver(javaParserTypeSolver));

        CompilationUnit cu = StaticJavaParser.parse(implementingTypePath);
        ClassOrInterfaceDeclaration implementingTypeClass = Navigator.demandClass(cu, "A");

        // Attempt to resolve `DuplicateTypeName` from `class ImplementingType implements **DuplicateTypeName**`
        assumeTrue(implementingTypeClass.getImplementedTypes().size() > 0);
        ClassOrInterfaceType implementedType = implementingTypeClass.getImplementedTypes(0);
        ResolvedReferenceType resolvedImplementedType = implementedType.resolve().asReferenceType();

        // Verify qualified name matches the non-nested class _in the same package.
        // Note verbose assertions show both the "correct" expected value, and the erroneous value to be avoided.
        string qualifiedName = resolvedImplementedType.getQualifiedName();
        assertEquals("implements_duplicate.DuplicateTypeName", qualifiedName, "Error - not resolved to interface _in the package.");
        assertNotEquals("implements_duplicate.A.DuplicateTypeName", qualifiedName, "Error - mistakenly resolved to a nested class instead of the expected interface.");
    }

    [TestMethod]
    void testTypesWithSameNameStaticNonTypeAndNonStaticType(){
        Path srcRootPath = adaptPath("src/test/resources/TypeResolutionWithSameNameTest/02_ignore_static_non_type_import");
        Path mainPath = adaptPath("src/test/resources/TypeResolutionWithSameNameTest/02_ignore_static_non_type_import/main/Main.java");

        JavaParserTypeSolver javaParserTypeSolver = new JavaParserTypeSolver(srcRootPath);
        StaticJavaParser
                .getConfiguration()
                .setSymbolResolver(new JavaSymbolSolver(javaParserTypeSolver));

        CompilationUnit cu = StaticJavaParser.parse(mainPath);
        ClassOrInterfaceDeclaration c = Navigator.demandClass(cu, "Main");

        string qualifiedName = c.getFieldByName("field_a").get().resolve().getType().describe();
        assertEquals("another.A", qualifiedName, "Error - not resolved to a class.");
        assertNotEquals("another.MyEnum.A", qualifiedName, "Error - mistakenly resolved to an enum member instead of the expected class.");
    }

    [TestMethod]
    void testTypesWithSameNameSingleTypeImportAndPackage(){
        Path srcRootPath = adaptPath("src/test/resources/TypeResolutionWithSameNameTest/03_single_type_import_precedes_package_member");
        Path mainPath = adaptPath("src/test/resources/TypeResolutionWithSameNameTest/03_single_type_import_precedes_package_member/main/Main.java");

        JavaParserTypeSolver javaParserTypeSolver = new JavaParserTypeSolver(srcRootPath);
        StaticJavaParser
                .getConfiguration()
                .setSymbolResolver(new JavaSymbolSolver(javaParserTypeSolver));

        CompilationUnit cu = StaticJavaParser.parse(mainPath);
        ClassOrInterfaceDeclaration c = Navigator.demandClass(cu, "Main");

        string qualifiedName = c.getFieldByName("field_a").get().resolve().getType().describe();
        assertEquals("another.A", qualifiedName, "Error - not resolved to the imorted class.");
        assertNotEquals("main.A", qualifiedName, "Error - mistakenly resolved to a package member insted of the explicitly imported class.");
    }

    [TestMethod]
    void testTypesWithSameNamePackageAndAsteriskImport(){
        Path srcRootPath = adaptPath("src/test/resources/TypeResolutionWithSameNameTest/04_package_member_precedes_asterisk_import");
        Path mainPath = adaptPath("src/test/resources/TypeResolutionWithSameNameTest/04_package_member_precedes_asterisk_import/main/Main.java");

        JavaParserTypeSolver javaParserTypeSolver = new JavaParserTypeSolver(srcRootPath);
        StaticJavaParser
                .getConfiguration()
                .setSymbolResolver(new JavaSymbolSolver(javaParserTypeSolver));

        CompilationUnit cu = StaticJavaParser.parse(mainPath);
        ClassOrInterfaceDeclaration c = Navigator.demandClass(cu, "Main");

        string qualifiedName = c.getFieldByName("field_a").get().resolve().getType().describe();
        assertEquals("main.A", qualifiedName, "Error - not resolved to a package member.");
        assertNotEquals("another.A", qualifiedName, "Error - mistakenly resolved to an asterisk-imported class instead of the expected package member.");
    }

    [TestMethod]
    void testTypesWithSameNameAsteriskImportAndJavaLang(){
        Path srcRootPath = adaptPath("src/test/resources/TypeResolutionWithSameNameTest/05_asterisk_import_precedes_java_lang");
        Path mainPath = adaptPath("src/test/resources/TypeResolutionWithSameNameTest/05_asterisk_import_precedes_java_lang/main/Main.java");

        JavaParserTypeSolver javaParserTypeSolver = new JavaParserTypeSolver(srcRootPath);
        StaticJavaParser
                .getConfiguration()
                .setSymbolResolver(new JavaSymbolSolver(javaParserTypeSolver));

        CompilationUnit cu = StaticJavaParser.parse(mainPath);
        ClassOrInterfaceDeclaration c = Navigator.demandClass(cu, "Main");

        string qualifiedName = c.getFieldByName("s").get().resolve().getType().describe();
        assertEquals("another.String", qualifiedName, "Error - not resolved to an asterisk-imported class.");
        assertNotEquals("java.lang.String", qualifiedName, "Error - mistakenly resolved to a member of java.lang instead of a member of asterisk-imported package.");
    }

    [TestMethod]
    void testTypesWithSameNameInPackageAndNestedMethodDeclaration() {
        string code = "package implements_duplicate;\n" +
            "\n" +
            "import java.util.Formattable;\n" +
            "\n" +
            "public abstract class A implements Formattable {\n" +
            "\n" +
            "    public interface Formattable {\n" +
            "    }\n" +
            "\n" +
            "    public void foo(Formattable f) {\n" +
            "    }\n" +
            "\n" +
            "}\n";

        StaticJavaParser
                .getConfiguration()
                .setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver(false)));

        CompilationUnit cu = StaticJavaParser.parse(code);

        MethodDeclaration decl = cu.findFirst(MethodDeclaration.class).get();
        string qualifiedName = decl.getParameters().get(0).getType().resolve().asReferenceType().getQualifiedName();
        assertEquals("implements_duplicate.A.Formattable", qualifiedName, "Error - not resolved to local interface");
        assertNotEquals("java.util.Formattable", qualifiedName,
                        "Error - mistakenly resolved to import used _in implements");
    }
}
