using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Utilidades.Helpers
{
  public static class DataTableExtensions
  {
    /// <summary>
    /// Convierte un tipo específico a una DataTable
    /// </summary>
    /// <typeparam name="T">El tipo del cual se quiere convertir</typeparam>
    /// <param name="listado">La lista de la cual obtener los parámetros</param>
    /// <param name="nombreTabla">El nombre de la tabla, de no incorporarse, se pondrá el nombre T</param>
    public static DataTable ToDataTable<T>(this IEnumerable<T> listado, string nombreTabla = null) where T : class
    {
      if(listado is null) return null;
      Type tipoBase = typeof(T);
      //El nombre puede ser cualquiera en caso que no se escriba
      DataTable dTable = new DataTable(string.IsNullOrWhiteSpace(nombreTabla) ? tipoBase.Name : nombreTabla);
      var propiedades = ObtenerPropiedades<T>();// tipoBase.ObtenerPropiedades();
      AgregarColumnas(propiedades, dTable.Columns);
      if(!listado.Any()) //Si no hay nada, solo regresa el DataTable sin elementos
        return dTable;
      AgregarFilas(listado, propiedades, dTable);
      return dTable;
    }

    /// <summary>
    /// Obtiene las propiedades de un tipo
    /// </summary>
    public static PropertyInfo[] ObtenerPropiedades<T>() where T : class
      => typeof(T).ObtenerPropiedades();

    /// <summary>
    /// Obtiene las propiedades de un tipo, usando como base un objeto
    /// </summary>
    /// <param name="type">El objeto del cual se buscarán las propiedades</param>
    public static PropertyInfo[] ObtenerPropiedades(this object type)
    {
      if(type is null) return null; //También se puede enviar un ArgumentException
      if(type is Type tp) return tp.GetProperties(BindingFlags.Public | BindingFlags.Instance);
      else return type.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    /// <summary>
    /// Agrega las columnas al DataTable.Columns
    /// </summary>
    /// <param name="properties">Las propiedades</param>
    /// <param name="columns">La colección de columnas (Usualmente dataTable.Columns)</param>
    public static void AgregarColumnas(PropertyInfo[] properties, DataColumnCollection columns)
    {
      if(columns is null) return; //Podría lanzar una excepción en vez de solo retornar
      foreach(var item in properties)
      {
        Type t = item.PropertyType;
        bool aceptaNulos = t.AceptaNulos();
        if(aceptaNulos) t = Nullable.GetUnderlyingType(t);// Type.GetType(t.GenericTypeArguments[0].FullName);
        columns.Add(item.Name, t).AllowDBNull = aceptaNulos;
      }
    }

    /// <summary>
    /// Agrega las filas al DataTable.Rows
    /// </summary>
    /// <typeparam name="T">El tipo de la lista</typeparam>
    /// <param name="items">La lista</param>
    /// <param name="properties">Las propiedades</param>
    /// <param name="table">El DataTable</param>
    private static void AgregarFilas<T>(IEnumerable<T> items, PropertyInfo[] properties, DataTable table)
    {
      if(table is null || properties is null || items is null) return;
      foreach(var item in items)
        AgregarFila(item, properties, table);
    }

    /// <summary>
    /// Agrega una fila individual a lo que ya haya en el DataTable
    /// </summary>
    /// <typeparam name="T">El tipo de la lista</typeparam>
    /// <param name="item">El valor que se agregará</param>
    /// <param name="properties">Las propiedades</param>
    /// <param name="table">El DataTable</param>
    private static void AgregarFila<T>(T item, PropertyInfo[] properties, DataTable table)
    {
      //Si se requiere una mejor prevención
      //if(table is null || properties is null || item is null) return;
      var row = table.NewRow();
      foreach(PropertyInfo prop in properties)
        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
      table.Rows.Add(row);
    }

    /// <summary>
    /// Si algún tipo en concreto acepta o no valores null
    /// </summary>
    /// <typeparam name="T">El tipo en concreto xD</typeparam>
    public static bool AceptaNulos<T>() => default(T) is null;

    /// <summary>
    /// Si algún objeto acepta valores null o no
    /// </summary>
    /// <param name="o">El objeto xD</param>
    public static bool AceptaNulos(this object o)
    {
      if(o is null) return false;
      if(o is Type tp) return Nullable.GetUnderlyingType(tp) != null;
      else return Nullable.GetUnderlyingType(o.GetType()) != null;
    }
  }
}
