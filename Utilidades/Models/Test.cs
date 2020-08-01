using System;

namespace Utilidades.Models
{
  public class Test
  {
    public uint Id { get; set; }
    public string Nombre { get; set; }
    public DateTime Fecha { get; set; }
    public DateTime? FechaConNull { get; set; }
    public decimal? Valor { get; set; }
    public uint? OtroValor { get; set; }
  }
}
