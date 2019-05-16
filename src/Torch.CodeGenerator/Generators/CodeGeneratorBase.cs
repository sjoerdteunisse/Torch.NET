﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Torch.CodeGenerator.Models;

namespace Torch.CodeGenerator
{
    public abstract class CodeGeneratorBase
    {
        // generate an entire API function declaration
        protected virtual void GenerateApiFunction(Declaration decl, StringBuilder s)
        {
            var retval = GenerateReturnType(decl);
            var arguments = GenerateArguments(decl);
            GenerateDocString(decl, s);
            string declare = $"public {retval} {decl.name}({arguments})";
            s.AppendLine(declare);
            s.AppendLine("{");
            GenerateBody(decl, s);
            s.AppendLine("}\r\n");

            Console.WriteLine(declare);
        }

        // generate the argument list between the parentheses of a generated API function
        protected virtual string GenerateArguments(Declaration decl)
        {
            var s = new StringBuilder();
            int i = 0;
            var args = ExpandArguments(decl.arguments).ToArray();
            foreach (var arg in args)
            {
                // TODO modifier (if any)
                // parameter type
                s.Append(MapType(arg));
                if (arg.is_nullable)
                    s.Append("?");
                s.Append(" ");
                // parameter name
                s.Append(EscapeName(arg.name));
                if (!string.IsNullOrWhiteSpace(arg.@default))
                    s.Append($"={MapDefaultValue(arg.@default)}");
                i++;
                if (i < args.Length)
                    s.Append(", ");
            }
            return s.ToString();
        }

        // this expands certain types into inline arguments 
        protected virtual IEnumerable<Argument> ExpandArguments(List<Argument> args)
        {
            foreach (var arg in args)
            {
                switch (arg.dynamic_type)
                {
                    case "TensorOptions":
                        yield return new Argument() { type = "ScalarType", name = "dtype", @default = "None", is_nullable = true };
                        yield return new Argument() { type = "Layout", name = "layout", @default = "None", is_nullable = true };
                        yield return new Argument() { type = "Device", name = "device", @default = "None", is_nullable = true };
                        yield return new Argument() { type = "bool", name = "pin_memory", @default = "None", is_nullable = true };
                        break;
                    default:
                        yield return arg;
                        break;
                }
            }
        }

        // maps None to null, etc
        protected string MapDefaultValue(string @default)
        {
            switch (@default)
            {
                case "None": return "null";
                case "True": return "true";
                case "False": return "false";
            }
            return @default;
        }

        // list of c# keywords that are not allowed as variable names or parameter names
        protected readonly HashSet<string> _disallowed_names = new HashSet<string>()
        {
            "abstract", "as", "base", "bool", "break",         "byte", "case", "catch", "char", "checked",         "class",   "const",   "continue", "decimal", "default",         "delegate",    "do", "double",  "else", "enum",         "event",   "explicit", "extern", "false", "finally",         "fixed", "float",   "for", "foreach", "goto",         "if", "implicit", "in", "int", "interface",         "internal", "is", "lock", "long", "namespace",         "new", "null", "object", "operator",    "out",         "override", "params",  "private", "protected", "public",             "readonly", "ref", "return", "sbyte", "sealed",             "short", "sizeof", "stackalloc", "static", "string",         "struct",  "switch", "this",    "throw", "true",             "try", "typeof", "uint",    "ulong",   "unchecked",         "unsafe", "ushort", "using", "var", "virtual",             "void", "volatile", "while",         "add", "alias",   "async", "await", "dynamic",         "get", "global",  "nameof",  "partial", "remove",             "set", "value", "when",    "where", "yield",         "ascending", "by",  "descending", "equals",  "from",             "group",   "in", "into", "join",    "let",             "on",  "orderby", "select",  "where"
        };

        // escape a varibale name if it violates C# syntax
        protected string EscapeName(string name)
        {
            if (_disallowed_names.Contains(name))
                return "@" + name;
            return name;
        }

        // generates the return type declaration of a generated API function declaration
        protected virtual string GenerateReturnType(Declaration decl)
        {
            if (decl.returns == null || decl.returns.Count == 0)
                return "void";
            else if (decl.returns.Count == 1)
            {
                return MapType(decl.returns[0]);
            }
            else
            {
                throw new NotImplementedException("Return tuple");
            }
        }

        // maps a C++ type to C# type
        protected string MapType(Argument arg)
        {
            switch (arg.type)
            {
                // basic types
                case "bool": return "bool";
                case "int": return "int";
                case "int64_t": return "long";
                case "double": return "double";
                case "string": return "string";
                case "Object": return "object";
                // sequence types
                case "IntArrayRef":
                    if (arg.name == "size")
                        return "NumSharp.Shape"; // <-- int[] size usually means Shape of the tensor. 
                    return "int[]";
                // torch types
                case "ScalarType": return "Torch.ScalarType";
                case "Layout": return "Torch.Layout";
                case "Device": return "Torch.Device";
                case "Tensor": return "Torch.Tensor";
                default:
                    // Console.WriteLine("MapType doesn't handle type: " + arg.type);
                    return arg.type;
            }
        }

        protected virtual void GenerateDocString(Declaration decl, StringBuilder s)
        {
            // TODO: generate xml doc strings from _torch_docs.py
        }

        // generates only the body of the API function declaration
        protected virtual void GenerateBody(Declaration decl, StringBuilder s)
        {
            s.AppendLine("    //auto-generated code, do not change");
            // first generate the Python objects for every argument
            foreach (var arg in ExpandArguments(decl.arguments))
            {
                s.AppendLine($"    var _{arg.name} = Util.ToPython({EscapeName(arg.name)});");
            }
            // then call the function
            s.AppendLine($"    dynamic py = torch.{decl.name}({string.Join(", ", decl.arguments.Select(a => "_" + a.name))});");
            // return the return value if any
            if (decl.returns.Count == 0)
                return;
            if (decl.returns.Count == 1)
                s.AppendLine($"    return Util.ToCsharp<{decl.returns[0].type}>(py);");
            else
            {
                throw new NotImplementedException("return a tuple or array of return values");
            }
        }

        protected string InferDataType(string value, string hint)
        {
            if (value == "(array_like)") return "NDArray";
            if (value == "(int)") return "int";

            if (hint.ToLower().Contains("number of "))
                return "int";

            return "string";
        }
    }
}