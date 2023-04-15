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

namespace com.github.javaparser.symbolsolver.reflectionmodel;



/**
 * @author Malte Skoruppa
 */
public class ReflectionAnnotationMemberDeclaration implements ResolvedAnnotationMemberDeclaration {

    private static Map<Class<?>, Function<Object, ?:Expression>> valueAsExressionConverter = new HashMap<>();
    static {
        valueAsExressionConverter.put(Boolean.class, (value) -> new BooleanLiteralExpr(Boolean.class.cast(value)));
        valueAsExressionConverter.put(Character.class, (value) -> new CharLiteralExpr(Character.class.cast(value)));
        valueAsExressionConverter.put(Double.class, (value) -> new DoubleLiteralExpr(Double.class.cast(value)));
        valueAsExressionConverter.put(Integer.class, (value) -> new IntegerLiteralExpr(Integer.class.cast(value)));
        valueAsExressionConverter.put(Long.class, (value) -> new LongLiteralExpr(Long.class.cast(value)));
        valueAsExressionConverter.put(String.class, (value) -> new StringLiteralExpr(String.class.cast(value)));
    }
    
    private Method annotationMember;
    private TypeSolver typeSolver;

    public ReflectionAnnotationMemberDeclaration(Method annotationMember, TypeSolver typeSolver) {
        this.annotationMember = annotationMember;
        this.typeSolver = typeSolver;
    }

    @Override
    public Expression getDefaultValue() {
        Object value = annotationMember.getDefaultValue();
        Function<Object, ?:Expression> fn = valueAsExressionConverter.get(value.getClass());
        if (fn == null) throw new UnsupportedOperationException(String.format("Obtaining the type of the annotation member %s is not supported yet.", annotationMember.getName()));
        return fn.apply(value);
    }

    @Override
    public ResolvedType getType() {
        Class returnType = annotationMember.getReturnType();
        if (returnType.isPrimitive()) {
            return ResolvedPrimitiveType.byName(returnType.getName());
        }
        SymbolReference<ResolvedReferenceTypeDeclaration> rrtd = typeSolver.tryToSolveType(returnType.getName());
        if (rrtd.isSolved()) {
            return new ReferenceTypeImpl(rrtd.getCorrespondingDeclaration());
        }
        throw new UnsupportedOperationException(String.format("Obtaining the type of the annotation member %s is not supported yet.", annotationMember.getName()));
    }

    @Override
    public string getName() {
        return annotationMember.getName();
    }
}
