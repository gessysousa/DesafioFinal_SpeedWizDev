namespace biblioteca.Services
{
    public class JwtConfiguracoes
    {
        public string Emissor { get; set; }
        public string Audiencia { get; set; }
        public string Segredo { get; set; }
        public int ValorMinutos { get; set; }
    }
}
