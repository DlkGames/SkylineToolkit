using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text;

namespace SkylineToolkit.Debugging
{
    internal class CodeRunner
    {
        private string[] namespaces;

        public CodeRunner(string[] usingNamespaces = null)
        {
            if (usingNamespaces == null)
            {
                usingNamespaces= new string[] { "SkylineToolkit", "SkylineToolkit.UI" };
            }

            this.namespaces = usingNamespaces;
        }

        public object RunCode(string code)
        {
            object result = null;

            try
            {
                int evaluationNumber = DateTime.Now.GetHashCode();

                string generatedCode = this.GenerateCode(code, evaluationNumber);

                Assembly compiledAssembly = this.CompileCode(generatedCode);
                
                if (compiledAssembly != null)
                {
                    result = RunCompiledCode(compiledAssembly, evaluationNumber);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed to evaluate code: \n" + code);
                Log.Exception(ex);
            }

            return result;
        }

        private static object RunCompiledCode(Assembly compiledCode, int evaluationNumber)
        {
            object result = null;

            object instance = compiledCode.CreateInstance("SkylineToolkitDebugging.EvaluationClass" + evaluationNumber);

            Module[] modules = compiledCode.GetModules(false);
            Type[] types = modules[0].GetTypes();

            foreach (Type type in types)
            {
                if (type.Name == "EvaluationClass" + evaluationNumber)
                {
                    MethodInfo method = type.GetMethod("EvaluationMethod");

                    result = method.Invoke(instance, null);
                }
            }

            return result;
        }

        private string GenerateCode(string code, int evaluationNumber)
        {
            StringBuilder codeBuilder = new StringBuilder();

            codeBuilder.AppendLine("using SkylineToolkit.Debugging;");
            codeBuilder.AppendLine("using System;");
            codeBuilder.AppendLine("using UnityEngine;");

            foreach (string ns in this.namespaces)
            {
                codeBuilder.AppendLine("using " + ns + ";");
            }

            codeBuilder.AppendLine("namespace SkylineToolkitDebugging {");
            codeBuilder.AppendLine("public class EvaluationClass" + evaluationNumber + "{");
            codeBuilder.AppendLine("public object EvaluationMethod() {" + code + "; return null; }");
            codeBuilder.AppendLine("}}");

            return codeBuilder.ToString();
        }

        private Assembly CompileCode(string code)
        {
            string options = "/target:library /optimize ";
            options += "/lib:\"" + Path.Combine(SkylinePath.Application, "Cities_Data\\Managed") + "\" ";

            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("mscorlib.dll");
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            parameters.ReferencedAssemblies.Add("ICities.dll");
            parameters.ReferencedAssemblies.Add("UnityEngine.dll");
            parameters.ReferencedAssemblies.Add("Assembly-CSharp.dll");
            parameters.ReferencedAssemblies.Add("Assembly-CSharp-firstpass.dll");

            foreach (string directory in Directory.GetDirectories(SkylinePath.Mods, "*", SearchOption.TopDirectoryOnly))
            {
                options += "/lib:\"" + directory + "\" ";

                foreach (string file in Directory.GetFiles(directory, "*.dll", SearchOption.TopDirectoryOnly))
                {
                    parameters.ReferencedAssemblies.Add(Path.GetFileName(file));
                }
            }

            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            parameters.IncludeDebugInformation = false;
            parameters.CompilerOptions = options;

            Assembly colossalManaged = typeof(ColossalFramework.Plugins.PluginManager).Assembly;

            if (colossalManaged == null)
            {
                Log.Error("Unable to resolve ColossalManaged.dll");

                return null;
            }

            Type codeProviderType = colossalManaged.GetType("ColossalFramework.Plugins.ColossalCSharpCodeProvider");

            if (codeProviderType == null)
            {
                Log.Error("Unable to resolve ColossalCSharpCodeProvider type.");
            }

            ConstructorInfo ctor = codeProviderType.GetConstructor(new Type[0]);

            //CodeDomProvider codeProvider = new CSharpCodeProvider();
            CodeDomProvider codeProvider = (CodeDomProvider)ctor.Invoke(new object[0]);

            CompilerResults compilerResults = codeProvider.CompileAssemblyFromSource(parameters, code);

            if (compilerResults.NativeCompilerReturnValue != 0)
            {
                //Log.Error("Failed to compile code: {0}", code);
                // TODO Logging methods for blank strings and values (int, double, ... ) see Console.WriteLine(..);

                DumpCompilerErrors(compilerResults);

                return null;
            }
            else
            {
                return compilerResults.CompiledAssembly;
            }
        }

        private void DumpCompilerErrors(CompilerResults results)
        {
            Log.Error("Number of errors: " + results.Errors.Count);

            foreach (CompilerError error in results.Errors)
            {
                Log.Error("{0}:{1} -> {2}", error.Line, error.Column, error.ErrorText);
            }
        }

        private class CustomCSharpCodeProvider : CSharpCodeProvider
        {

        }
    }
}
