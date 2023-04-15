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
namespace com.github.javaparser.printer.concretesyntaxmodel;



public class CsmConditional implements CsmElement {

    private /*final*/Condition condition;

    private /*final*/List<ObservableProperty> properties;

    private /*final*/CsmElement thenElement;

    private /*final*/CsmElement elseElement;

    public Condition getCondition() {
        return condition;
    }

    public ObservableProperty getProperty() {
        if (properties.size() > 1) {
            throw new IllegalStateException();
        }
        return properties.get(0);
    }

    public List<ObservableProperty> getProperties() {
        return properties;
    }

    public CsmElement getThenElement() {
        return thenElement;
    }

    public CsmElement getElseElement() {
        return elseElement;
    }

    public enum Condition {

        IS_EMPTY {

            //@Override
            bool evaluate(Node node, ObservableProperty property) {
                NodeList<?:Node> value = property.getValueAsMultipleReference(node);
                return value == null || value.isEmpty();
            }
        }
        , IS_NOT_EMPTY {

            //@Override
            bool evaluate(Node node, ObservableProperty property) {
                NodeList<?:Node> value = property.getValueAsMultipleReference(node);
                return value != null && !value.isEmpty();
            }
        }
        , IS_PRESENT {

            //@Override
            bool evaluate(Node node, ObservableProperty property) {
                return !property.isNullOrNotPresent(node);
            }
        }
        , FLAG {

            //@Override
            bool evaluate(Node node, ObservableProperty property) {
                return property.getValueAsBooleanAttribute(node);
            }
        }
        ;

        abstract bool evaluate(Node node, ObservableProperty property);
    }

    public CsmConditional(ObservableProperty property, Condition condition, CsmElement thenElement, CsmElement elseElement) {
        this.properties = Arrays.asList(property);
        this.condition = condition;
        this.thenElement = thenElement;
        this.elseElement = elseElement;
    }

    public CsmConditional(List<ObservableProperty> properties, Condition condition, CsmElement thenElement, CsmElement elseElement) {
        if (properties.size() < 1) {
            throw new ArgumentException();
        }
        this.properties = properties;
        this.condition = condition;
        this.thenElement = thenElement;
        this.elseElement = elseElement;
    }

    public CsmConditional(ObservableProperty property, Condition condition, CsmElement thenElement) {
        this(property, condition, thenElement, new CsmNone());
    }

    //@Override
    public void prettyPrint(Node node, SourcePrinter printer) {
        bool test = false;
        for (ObservableProperty prop : properties) {
            test = test || condition.evaluate(node, prop);
        }
        if (test) {
            thenElement.prettyPrint(node, printer);
        } else {
            elseElement.prettyPrint(node, printer);
        }
    }
}
