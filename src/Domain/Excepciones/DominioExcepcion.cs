namespace Domain.Excepciones;

public abstract class DominioExcepcion : Exception
{
    protected DominioExcepcion(string mensaje) 
        : base(mensaje){}
}