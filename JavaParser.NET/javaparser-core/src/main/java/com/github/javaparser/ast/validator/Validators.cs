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
namespace com.github.javaparser.ast.validator;



/**
 * A validator that will call a collection of validators.
 */
public class Validators implements Validator {

    private /*final*/List<Validator> validators = new ArrayList<>();

    public Validators(Validator... validators) {
        this.validators.addAll(Arrays.asList(validators));
    }

    public List<Validator> getValidators() {
        return validators;
    }

    public Validators remove(Validator validator) {
        if (!validators.remove(validator)) {
            throw new AssertionError("Trying to remove a validator that isn't there.");
        }
        return this;
    }

    public Validators replace(Validator oldValidator, Validator newValidator) {
        remove(oldValidator);
        add(newValidator);
        return this;
    }

    public Validators add(Validator newValidator) {
        validators.add(newValidator);
        return this;
    }

    //@Override
    public void accept(Node node, ProblemReporter problemReporter) {
        validators.forEach(v -> v.accept(node, problemReporter));
    }
}
