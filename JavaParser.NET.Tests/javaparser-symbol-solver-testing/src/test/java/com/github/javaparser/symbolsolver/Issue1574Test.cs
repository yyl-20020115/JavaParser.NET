/*
 * Copyright (C) 2013-2023 The JavaParser Team.
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

namespace com.github.javaparser.symbolsolver;




public class Issue1574Test {
    private static /*final*/string LINE_FILE = "src/test/resources/issue1574/Comment.java";
    private static /*final*/string BLOCK_FILE = "src/test/resources/issue1574/BlockComment.java";
    private static /*final*/string ORPHAN_FILE = "src/test/resources/issue1574/ClassWithOrphanComments.java";
    [TestMethod]
    void removeAllCommentsBeforePackageLine(){
        CompilationUnit cu = StaticJavaParser.parse(new File(LINE_FILE));
        for(Comment child: cu.getComments()){
            child.remove();
        }
        assertEquals(0,cu.getComments().size());
        assertFalse(cu.getComment().isPresent());
    }
    [TestMethod]
    void removeAllCommentsBeforePackageBlock(){
        CompilationUnit cu = StaticJavaParser.parse(new File(BLOCK_FILE));
        for(Comment child: cu.getComments()){
            child.remove();
        }
        assertEquals(0,cu.getComments().size());
        assertFalse(cu.getComment().isPresent());
    }
    [TestMethod]
    void getAllContainedCommentBeforePackageDeclarationLine(){
        CompilationUnit cu = StaticJavaParser.parse(new File(LINE_FILE));
        List<Comment> comments = cu.getAllContainedComments();
        assertEquals(2,comments.size());

    }
    [TestMethod]
    void getAllContainedCommentBeforePackageDeclarationBlock(){
        CompilationUnit cu = StaticJavaParser.parse(new File(BLOCK_FILE));
        List<Comment> comments = cu.getAllContainedComments();
        assertEquals(2,comments.size());

    }
    [TestMethod]
    void getAllCommentBeforePackageDeclarationOrphan(){
        CompilationUnit cu = StaticJavaParser.parse(new File(ORPHAN_FILE));
        List<Comment> comments = cu.getAllContainedComments();
        assertEquals(6,comments.size());

    }
    [TestMethod]
    void getOrphanComments(){
        CompilationUnit cu = StaticJavaParser.parse(new File(LINE_FILE));
        List<Comment> comments = cu.getOrphanComments();
        //The 2 first should be orphan comment while the third will be associated to the package
        assertEquals(1,comments.size());


    }
    [TestMethod]
    void getOrphanCommentsBlock(){
        CompilationUnit cu = StaticJavaParser.parse(new File(BLOCK_FILE));
        List<Comment> comments = cu.getOrphanComments();
        //The 2 first should be orphan comment while the third will be associated to the package
        assertEquals(1,comments.size());

    }
    [TestMethod]
    void getAllCommentBeforePackageDeclarationLine(){
        CompilationUnit cu = StaticJavaParser.parse(new File(LINE_FILE));
        List<Comment> comments = cu.getComments();
        assertEquals(3,comments.size());

    }
    [TestMethod]
    void getAllCommentBeforePackageDeclarationBlock(){
        CompilationUnit cu = StaticJavaParser.parse(new File(BLOCK_FILE));
        List<Comment> comments = cu.getComments();
        assertEquals(3,comments.size());
    }

}
