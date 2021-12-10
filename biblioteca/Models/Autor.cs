using System;

namespace biblioteca.Models
{
    public class Autor
    {
        #region Propriedades
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public DateTime CriadoEm { get; set; }
        #endregion
    }
}
