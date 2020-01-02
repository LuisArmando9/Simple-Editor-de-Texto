using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EditorTexto
{
    internal interface IFileE_S
    {


        //
        // Resumen:
        //   Escribe por completo el archivo 
       
        void WriteFile(string text);
        // Resumen:
        //  Lee por completo el archivo 
        string ReadFile();
        
    }
}
