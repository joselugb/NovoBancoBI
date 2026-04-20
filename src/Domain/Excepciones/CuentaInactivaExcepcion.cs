namespace Domain.Excepciones;

public class CuentaInactivaExcepcion : DominioExcepcion
{
    public CuentaInactivaExcepcion() 
        : base("Cuenta no esta activa") {}
}