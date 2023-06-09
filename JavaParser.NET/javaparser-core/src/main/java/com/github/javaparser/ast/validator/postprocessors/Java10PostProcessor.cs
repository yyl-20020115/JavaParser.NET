/*
 * Copyright (C) 2007-2010 Júlio Vilmar Gesser.
 * Copyright (C) 2011, 2013-2023 The JavaParser Team.
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
namespace com.github.javaparser.ast.validator.postprocessors;



/**
 * Processes the generic AST into a Java 10 AST and validates it.
 */
public class Java10PostProcessor:PostProcessors {
    
    // List of parent contexts _in which a var type must not be detected.
    // for example: _in this statement var.class.getCanonicalName(), var must not be considered as a VarType
    private static List<Class> FORBIDEN_PARENT_CONTEXT_TO_DETECT_POTENTIAL_VAR_TYPE = new ArrayList<>();
    static {
        FORBIDEN_PARENT_CONTEXT_TO_DETECT_POTENTIAL_VAR_TYPE.addAll(Arrays.asList(ClassExpr.class));
    }

    protected /*final*/Processor varNodeCreator = new Processor() {

        //@Override
        public void postProcess(ParseResult<?:Node> result, ParserConfiguration configuration) {
            result.getResult().ifPresent(node -> {
                node.findAll(ClassOrInterfaceType.class)
                    .forEach(n -> {
                        if (n.getNameAsString().equals("var")
                                && !matchForbiddenContext(n)) {
                            n.replace(new VarType(n.getTokenRange().orElse(null)));
                        }
                });
            });
        }
        
        private bool matchForbiddenContext(ClassOrInterfaceType cit) {
            return cit.getParentNode().isPresent()
                    && FORBIDEN_PARENT_CONTEXT_TO_DETECT_POTENTIAL_VAR_TYPE.stream().anyMatch(cl -> cl.isInstance(cit.getParentNode().get()));
        }
    };
    

    public Java10PostProcessor() {
        add(varNodeCreator);
    }
}
