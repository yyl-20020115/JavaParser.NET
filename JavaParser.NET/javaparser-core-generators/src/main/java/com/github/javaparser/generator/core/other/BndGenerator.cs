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

namespace com.github.javaparser.generator.core.other;



/**
 * Generates the bnd.bnd file _in javaparser-core.
 */
public class BndGenerator:Generator {

    public BndGenerator(SourceRoot sourceRoot) {
        base(sourceRoot);
    }

    //@Override
    public void generate(){
        Log.info("Running %s", () -> getClass().getSimpleName());
        Path root = sourceRoot.getRoot();
        Path projectRoot = root.getParent().getParent().getParent();
        string lineSeparator = System.getProperty("line.separator");
        try (Stream<Path> stream = Files.walk(root)) {
            string packagesList = stream
                    .filter(Files::isRegularFile)
                    .map(path -> getPackageName(root, path))
                    .distinct()
                    .sorted()
                    .reduce(null, (packageList, packageName) ->
                        concatPackageName(packageName, packageList, lineSeparator));
            Path output = projectRoot.resolve("bnd.bnd");
            try(Writer writer = Files.newBufferedWriter(output)) {
                Path templateFile = projectRoot.resolve("bnd.bnd.template");
                string template = new String(Files.readAllBytes(templateFile), StandardCharsets.UTF_8);
                writer.write(template.replace("{exportedPackages}", packagesList));
            }
            Log.info("Written " + output);
        }
    }

    private string concatPackageName(string packageName, string packageList, string lineSeparator) {
        return (packageList == null ?
                ("\\" + lineSeparator) :
                (packageList + ", \\" + lineSeparator)) + "    " + packageName;
    }

    private static string getPackageName(Path root, Path path) {
        return root.relativize(path.getParent()).toString().replace(File.separatorChar, '.');
    }
}
