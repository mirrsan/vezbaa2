using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZadatakNeki.DTO;
using ZadatakNeki.Models;

namespace ZadatakNeki.Controllers
{
    [Route("[controller]/[action]")]
    public class UredjajController : Controller
    {
        private readonly ToDoContext _context;

        public UredjajController(ToDoContext context)
        {
            _context = context;
        }

        // akcija koja vraca sve kancelarije
        [HttpGet]
        public IActionResult Svi()
        {
            return Ok(from k in _context.Uredjaji select new UredjajDTO { Naziv = k.Naziv });
        }

        // akcija koja vraca samo entitet koji ima dati ID
        [HttpGet("{id}:int")]
        public IActionResult PoId(long id)
        {
            var kan = from k in _context.Uredjaji
                      where k.Id == id
                      select new UredjajDTO { Naziv = k.Naziv };

            return Ok(kan.ToList());
        }

        // akcija koja upisuje novi entitet u bazu
        [HttpPost]
        public IActionResult Upisivanje(UredjajDTO uredjajNovi)
        {
            if (uredjajNovi == null)
            {
                return NotFound("ahahah nesto si zaebo");
            }

            Uredjaj uredjaj = new Uredjaj() { Naziv = uredjajNovi.Naziv };

            _context.Uredjaji.Add(uredjaj);
            _context.SaveChanges();

            return Ok("Dobro je brate, znaci sacuvao sam nista ne brini.");
        }

        // akcija koja menja postojeci entitet koji ima dati ID
        [HttpPut("{id}:int")]
        public IActionResult IzmenaPodataka(long id, UredjajDTO uredjaj)
        {
            Uredjaj stariUredjaj = _context.Uredjaji.Find(id);
            stariUredjaj.Naziv = uredjaj.Naziv;

            _context.SaveChanges();
            return Ok("Promjenjeno!");
        }

        // akcija koja brise entitet koji ima dati ID
        [HttpDelete("{id}")]
        public IActionResult Brisanje(long id)
        {
            Uredjaj uredjaj = _context.Uredjaji.Find(id);

            if (uredjaj == null)
            {
                return NotFound();
            }
            _context.Uredjaji.Remove(uredjaj);
            _context.SaveChanges();

            return Ok("Kancelarija je izbrisana iz baze podataka.");
        }

        // akcija koja pretrazuje entitet po imenu osobe
        [HttpGet("{naziv}")]
        public IActionResult PretragaPoImenu(string naziv)
        {
            var uredjaj = from n in _context.Uredjaji where n.Naziv == naziv select new { Naziv = n.Naziv };
            if (uredjaj == null)
            {
                return NotFound();
            }
            return Ok(uredjaj.ToList());
        }
    }
}
