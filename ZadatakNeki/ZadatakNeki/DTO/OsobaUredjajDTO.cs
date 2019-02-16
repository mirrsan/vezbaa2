using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZadatakNeki.DTO
{
    public class OsobaUredjajDTO
    {
        public DateTime PocetakKoriscenja { get; set; }
        public DateTime? KrajKoriscenja { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Kancelarija { get; set; }
        public string NazivUredjaja { get; set; }
    }
}
