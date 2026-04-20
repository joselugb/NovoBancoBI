
namespace Domain.Entidades
{
    public class Transaccion
    {
        public Guid Id { get; set; }
        public decimal Monto { get; private set; }
        public string Referencia { get; set; } = string.Empty;
    }
}
