namespace JwtIdentityApi.Models
{
    public class ResultadoRegistroDto
    {
        
       
        public string? UsuarioId { get; set; }
        public bool Exitoso { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Errores { get; set; }=new List<string>();
    }
}
