using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MueveIMGs
{
    class Program
    {
        static CommonOpenFileDialog carpetaOr = new CommonOpenFileDialog();

        static CommonOpenFileDialog carpetaDest = new CommonOpenFileDialog();
        [STAThread]
        static void Main(string[] args)
        {
            carpetaOr.Title = "Buscar carpeta que contiene archivos a mover";
            carpetaOr.IsFolderPicker = true;
            carpetaOr.Multiselect = true;
            CommonFileDialogResult accionUsuario = carpetaOr.ShowDialog();
            if (CommonFileDialogResult.Ok.Equals(accionUsuario))
            {
                IEnumerable<string> carpetas = carpetaOr.FileNames;
                foreach (string carpetaOrigen in carpetas)
                {
                    string[] contenidoOrigen = Directory.GetFiles(carpetaOrigen);
                    Console.WriteLine("{0} archivos encontrados\n Seleccione carpeta de destino.", contenidoOrigen.Length);

                    carpetaDest.IsFolderPicker = true;
                    AbreCarpetaDestino:
                    accionUsuario = carpetaDest.ShowDialog();
                    if (CommonFileDialogResult.Ok.Equals(accionUsuario))
                    {
                        string carpetaDestino = carpetaDest.FileName;
                        if (carpetaOrigen.Equals(carpetaDestino))
                        {
                            Console.WriteLine("Carpeta origen y carpeta de destino deben ser diferentes. Vuelve a elegir otra vez.");
                            goto AbreCarpetaDestino;
                        }
                        Console.WriteLine("Indicar numero inicial de secuencia para renombrar archivos");
                        int correlativoArchivoDestino = int.Parse(Console.ReadLine());
                        Console.WriteLine("Secuencia comienza en {0}\n Moviendo archivos . . .", correlativoArchivoDestino);

                        foreach (string imgOrigen in contenidoOrigen)
                        {
                            string extension = imgOrigen.Split('\\').Last().Split('.').Last();
                            string nombreArchivoDestino = "00000000".Substring(0, 8 - correlativoArchivoDestino.ToString().Length) + correlativoArchivoDestino + "." + extension;
                            Console.WriteLine("{0} copiado como {1}", imgOrigen, nombreArchivoDestino);
                            try
                            {
                                if (imgOrigen.Split('\\').Last().Contains("thumbs.db"))
                                    File.Delete(imgOrigen);
                                else
                                    File.Copy(imgOrigen, carpetaDestino + '\\' + nombreArchivoDestino);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("No se pudo continuar...\n{0}", ex.Message);
                            }
                            correlativoArchivoDestino++;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Se debe seleccionar una carpeta de destino. Pulse cualquier tecla para volver a intentar o cierre la ventana.");
                        Console.ReadKey();
                        goto AbreCarpetaDestino;
                    }
                }
            }
            else
            {
                Console.WriteLine("No se hizo nada, cerrando aplicación.");
                Thread.Sleep(2300);
                Environment.Exit(0);
            }
            Console.WriteLine("Proceso Terminado.");
            Thread.Sleep(2300);
            Environment.Exit(0);
        }
    }
}
