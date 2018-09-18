using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsumirServicioSF
{
    class Program
    {
        
        static void Main(string[] args)
        {





            string lines;
            bool produccion = false;
            string prod_endpoint = "TimbradoEndpoint_PRODUCCION";
            string test_endpoint = "TimbradoEndpoint_TESTING";
            

        //Si recibe error 417 deberá descomentar la linea a continuación
        //System.Net.ServicePointManager.Expect100Continue = false;

        //El paquete o namespace en el que se encuentran las clases
        //será el que se define al agregar la referencia al WebService,
        //en este ejemplo es: com.sf.ws.Timbrado

        com.sf.ws.Timbrado.TimbradoPortTypeClient portClient = null;
            portClient = (produccion)
                ? new com.sf.ws.Timbrado.TimbradoPortTypeClient(prod_endpoint)
                : portClient = new com.sf.ws.Timbrado.TimbradoPortTypeClient(test_endpoint);

            try { 
            
                byte[] bytes = Encoding.UTF8.GetBytes(System.IO.File.ReadAllText(@"C:\Facturas\Factura7.xml"));
               
                System.Console.WriteLine("Sending request...");
                System.Console.WriteLine("EndPoint = " + portClient.Endpoint.Address);
                com.sf.ws.Timbrado.CFDICertificacion response = portClient.timbrar("testing@solucionfactible.com", "timbrado.SF.16672", bytes, false);

                Console.WriteLine(response.ToString());

                System.Console.WriteLine("Información de la transacción");
                System.Console.WriteLine(response.status);
                System.Console.WriteLine(response.mensaje);
                System.Console.WriteLine("Resultados recibidos" + response.resultados.Length);
                com.sf.ws.Timbrado.CFDIResultadoCertificacion[] resultados = response.resultados;

                

                foreach (var item in resultados)
                {

                    
                    Console.WriteLine(item.cadenaOriginal);
                    Console.WriteLine(item.certificadoSAT);
                    Console.WriteLine(item.cfdiTimbrado);
                    Console.WriteLine(item.fechaTimbrado);
                    Console.WriteLine(item.fechaTimbradoSpecified);
                    Console.WriteLine(item.mensaje);
                    Console.WriteLine(item.qrCode);
                    Console.WriteLine(item.selloSAT);
                    Console.WriteLine(item.status);
                    Console.WriteLine(item.statusSpecified);
                    Console.WriteLine("UUID: " + item.uuid);
                    Console.WriteLine(item.versionTFD);

                    lines = "CADENA ORIGINAL:" + item.cadenaOriginal + ';';
                    lines += "CERTIFICADO SAT:" + item.certificadoSAT + ';';
                    lines += "CFDI TIMBRADO:" + System.Text.Encoding.UTF8.GetString(item.cfdiTimbrado) + ';';
                    
                  

                    lines += "FECHA TIMBRADO:" + item.fechaTimbrado + ';';

                    lines += "FECHA TIMBRADO SPECIFIED:" + item.fechaTimbradoSpecified + ';';
                    lines += "MENSAJE:" + item.mensaje + ';';
                    lines += "QRCODE:" + item.qrCode + ';';
                    lines += "SELLO SAT:" + item.selloSAT + ';';
                    lines += "STATUS:" + item.status + ';';
                    lines += "ITEM SPECIFIED:" + item.statusSpecified + ';';
                    lines += "UUID:" + item.uuid + ';';
                    lines += "VERSION TFD:" + item.versionTFD; 

                    System.IO.File.WriteAllText(@"C:\Facturas\FacturaResult.txt", lines);

                }

                Console.WriteLine();





               
                //Clases a usar en cancelación:
                //com.sf.ws.Timbrado.CFDICancelacion
                //com.sf.ws.Timbrado.CFDIResultadoCancelacion

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.StackTrace);
            }

            Console.ReadKey();
        }
    }
}
