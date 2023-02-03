using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;

namespace ClassToJson
{
    internal class ClassGenearator
    {
        public object Genearator(string classObj)
        {

            string codeToCompile = $@"
            using System;
            using System.Collections.Generic;
            namespace Sameer
            {{
                {classObj}
            }}";

            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(codeToCompile);

            string assemblyName = Path.GetRandomFileName();
            var refPaths = new[] {
                typeof(object).GetTypeInfo().Assembly.Location,
                typeof(Console).GetTypeInfo().Assembly.Location,
                Path.Combine(Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location), "System.Runtime.dll")
            };
            MetadataReference[] references = refPaths.Select(r => MetadataReference.CreateFromFile(r)).ToArray();

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    //IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                    //    diagnostic.IsWarningAsError ||
                    //    diagnostic.Severity == DiagnosticSeverity.Error);

                    //foreach (Diagnostic diagnostic in failures)
                    //{
                    //    Console.Error.WriteLine("\t{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    //}
                    throw new FormatException("Provide Valid Class String.");
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);

                    Assembly assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
                    List<Type> typeList = assembly.GetTypes().Where(a => a.FullName.StartsWith("Sameer")).ToList();
                    if (typeList != null && typeList.Count > 0)
                    {
                        var responseObj = assembly.CreateInstance(typeList[0].FullName)!;
                        List<PropertyInfo> propertyList = responseObj.GetType().GetProperties().ToList();
                        for (int i = 0; i < propertyList.Count; i++)
                        {
                            for (int j = 1; j < typeList.Count; j++)
                            {
                                if (propertyList[i].PropertyType.Name == typeList[j].Name)
                                {
                                    object childObj = assembly.CreateInstance(typeList[j].FullName)!;
                                    PropertyInfo? propertyInfo = responseObj.GetType().GetProperty(propertyList[i].Name);
                                    propertyInfo.SetValue(responseObj, childObj);
                                }
                            }
                        }
                        return responseObj;
                        //return assembly.CreateInstance(typeList[0].FullName)!;
                    }
                }
            }
            return null;
        }
        public static void SetText(string text)
        {
            var powershell = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = $"-command \"Set-Clipboard -Value \\\"{text}\\\"\""
                }
            };
            powershell.Start();
            powershell.WaitForExit();
        }
    }
}
