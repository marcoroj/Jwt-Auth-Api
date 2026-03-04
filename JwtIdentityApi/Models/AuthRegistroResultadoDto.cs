namespace JwtIdentityApi.Models
{
    public class AuthRegistroResultadoDto
    {
        public bool Exitoso { get; set; }
        public RespuestaAutenticacionDto? RespuestaAuhenticacion { get; set; }
        public List<string> Errores { get; set; }=new List<string>();

    }
}
