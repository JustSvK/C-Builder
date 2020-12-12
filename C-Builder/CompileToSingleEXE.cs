using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

using System.Windows.Forms;

namespace C_Builder
{
    class CompileToSingleEXE
    {
        public static bool CompileFromSource(string Malabar, string Output)
        {

            CompilerParameters Parametres = new CompilerParameters();

            // We want an executable file on disk.
            Parametres.GenerateExecutable = true;
            // The location of the executable.
            Parametres.OutputAssembly = Output;

            // Compilation options.
            string options = "/optimize+ /target:winexe /unsafe";


            // Set the options.
            Parametres.CompilerOptions = options;
            // Don't show errors.
            Parametres.TreatWarningsAsErrors = false;

            // Add the references to the libraries
            Parametres.ReferencedAssemblies.Add("System.dll");
            // for exemple :
            Parametres.ReferencedAssemblies.Add("System.Data.dll");
            Parametres.ReferencedAssemblies.Add("System.Management.dll");
            Parametres.ReferencedAssemblies.Add("mscorlib.dll");
            // your ressources files
            Parametres.EmbeddedResources.Add("files.resources");
            // .NET 2.0
            Dictionary<string, string> ProviderOptions = new Dictionary<string, string>();
            ProviderOptions.Add("CompilerVersion", "v2.0");

            // Compile the code
            CompilerResults Results = new CSharpCodeProvider(ProviderOptions).CompileAssemblyFromSource(Parametres, Malabar);


            if (Results.Errors.Count > 0)
            {

                MessageBox.Show(string.Format("The compiler has encountered {0} errors",
                    Results.Errors.Count), "Errors while compiling", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);


                foreach (CompilerError Err in Results.Errors)
                {
                    MessageBox.Show(string.Format("{0}\nLine: {1} - Column: {2}\nFile: {3}", Err.ErrorText,
                        Err.Line, Err.Column, Err.FileName), "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;

            }
            else
            {

                return true;
            }

        }
    }
}
