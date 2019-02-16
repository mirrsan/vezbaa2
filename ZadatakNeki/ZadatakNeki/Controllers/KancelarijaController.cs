using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Remotion.Linq.Clauses;
using ZadatakNeki.DTO;
using ZadatakNeki.Models;

namespace ZadatakNeki.Controllers
{
    [Route("api/[controller]/[action]")]
    public class KancelarijaController : Controller
    {
        private readonly ToDoContext _context;

        public KancelarijaController(ToDoContext context)
        {
            _context = context;
        }

        // akcija koja vraca sve kancelarije
        [HttpGet]
        public IActionResult SveKancelarije()
        {
            return Ok(from k in _context.Kancelarije select new KancelarijaDTO {Opis = k.Opis});
        }

        // akcija koja vraca samo entitet koji ima dati ID
        [HttpGet("{id}:int")]
        public IActionResult PoId(long id)
        {
            var kan = from k in _context.Kancelarije
                where k.Id == id
                select new KancelarijaDTO {Opis = k.Opis};

            return Ok(kan.ToList());
        }

        // akcija koja upisuje novi entitet u bazu
        [HttpPost]
        public IActionResult Upisivanje(KancelarijaDTO kancelarijaNova)
        {
            if (kancelarijaNova == null)
            {
                return Ok("ahaha nesto si zaebo");
            }

            Kancelarija kancelarija = new Kancelarija() {Opis = kancelarijaNova.Opis};

            _context.Kancelarije.Add(kancelarija);
            _context.SaveChanges();

            return Ok("Dobro je brate, znaci sacuvao sam nista ne brini.");
        }

        // akcija koja menja postojeci entitet koji ima dati ID
        [HttpPut("{id}:int")]
        public IActionResult IzmenaPodataka(long id, KancelarijaDTO kancelarija)
        {
            Kancelarija staraKancelarija = _context.Kancelarije.Find(id);
            staraKancelarija.Opis = kancelarija.Opis;

            _context.SaveChanges();
            return Ok("Promjenjeno!");
        }

        // akcija koja brise entitet koji ima dati ID
        [HttpDelete("{id}")]
        public IActionResult Brisanje(long id)
        {
            Kancelarija kancelarija = _context.Kancelarije.Find(id);

            if (kancelarija == null)
            {
                return NotFound();
            }
            _context.Kancelarije.Remove(kancelarija);
            _context.SaveChanges();

            return Ok("Kancelarija je izbrisana iz baze podataka.");
        }

        // akcija koja pretrazuje entitet po imenu osobe
        [HttpGet("{opis}")]
        public IActionResult PretragaPoImenu(string opis)
        {
            var kancelarija = from n in _context.Kancelarije where n.Opis == opis select new {Naziv = n.Opis};
            if (kancelarija == null)
            {
                return NotFound();
            }
            return Ok(kancelarija.ToList());
        }
    }
}