using System;
using System.Collections.Generic;
using System.Data;
using Utilidades.Helpers;
using Utilidades.Models;

namespace Utilidades
{
  class Program
  {
    static void Main(string[] args)
    {
      List<Test> lT = new List<Test>()
      {
        new Test()
        {
          Id = 1, Fecha = DateTime.Now, Nombre = "Iván", OtroValor = null, Valor = 5m
        },
        new Test()
        {
          Id = 150, Fecha = DateTime.Now.AddDays(-50), Nombre = "Romeo", OtroValor = 510, Valor = null
        },
        new Test()
        {
          Id = 950, Fecha = DateTime.Now.AddMonths(-20), Nombre = "Pablo", OtroValor = null, Valor = 5m
        },
        new Test()
        {
          Id = 1848, Fecha = DateTime.Now.AddYears(-900), Nombre = "Alejandro", FechaConNull = DateTime.Now.AddMinutes(1500)
        },
      };

      var dataTable = lT.ToDataTable();
      foreach(DataRow item in dataTable.Rows)
      {
        foreach(DataColumn item2 in dataTable.Columns)
        {
          var objeto = item.Field<object>(item2);
          Console.Write($"{item2.ColumnName} => ");
          Console.Write($"{objeto ?? "NULL"} | ");
          Console.WriteLine($"{item2.DataType.Name ?? "N/A"} | DBNULL: {(item2.AllowDBNull ? "Sí" : "No")}");
        }
        Console.WriteLine("------------------------");
      }
      Console.BackgroundColor = ConsoleColor.Green;
      Console.ForegroundColor = ConsoleColor.Black;
      Console.WriteLine("DATATABLE CREADO");
      Console.ResetColor();
      Console.ReadLine();
    }
  }
}
