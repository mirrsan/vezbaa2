using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZadatakNeki.DTO;
using ZadatakNeki.Models;

namespace ZadatakNeki.Controllers
{
    [Route("api/[controller]/[action]")]
    public class OsobaController : Controller
    {
        private readonly ToDoContext _context;

        public OsobaController(ToDoContext context)
        {
            _context = context;
        }

        // metod koji vraca sve osobe
        [HttpGet]
        public IActionResult SveOsobe()
        {
            var svi = from o in _context.Osobe
                      select new { Ime = o.Ime, Prezime = o.Prezime, Kancelarija = o.Kancelarija.Opis };

            return Ok(svi);
        }

        //akcija koja vraca samo entitet koji ima dati ID
        [HttpGet("{id}")]
        public IActionResult OsobaPoId(long id)
        {
            var osoba = from o in _context.Osobe
                        where o.Id == id
                        select new OsobaDTO { Ime = o.Ime, Prezime = o.Prezime, Kancelarija = o.Kancelarija.Opis };
            return Ok(osoba);
        }

        //akcija koja upisuje novi entitet u bazu
        [HttpPost]
        public IActionResult UpisivanjeOsobe(OsobaDTO osobaInfo)
        {
            if(osobaInfo == null)
            {
                return BadRequest("Niste upisali podatke da valja!");
            }
            Osoba osoba = new Osoba()
            {
                Ime = osobaInfo.Ime,
                Prezime = osobaInfo.Prezime,
            };
            Kancelarija kancelarija = new Kancelarija()
            {
                Opis = osobaInfo.Kancelarija
            };
            osoba.Kancelarija = kancelarija;

            _context.Osobe.Add(osoba);
            _context.SaveChanges();
            return Ok("Osoba je sacuvana u bazi.");
        }

        // akcija koja menja postojeci entitet koji ima dati ID
        [HttpPut("{id}")]
        public IActionResult IzmenaOsobe(long id, OsobaDTO noviInfo)
        {
            Osoba stariInfo = _context.Osobe.Find(id);

            stariInfo.Ime = noviInfo.Ime;
            stariInfo.Prezime = noviInfo.Prezime;
            Kancelarija kancelarija = new Kancelarija()
            {
                Opis = noviInfo.Kancelarija
            };
            stariInfo.Kancelarija = kancelarija;
            _context.SaveChanges();

            return Ok("Sacuvane su izmene.");
        }

        // akcija koja brise entitet koji ima dati ID
        [HttpDelete("{id}")]
        public IActionResult BrisanjeOsobe(long id)
        {
            Osoba osoba = _context.Osobe.Find(id);

            if(osoba == null)
            {
                return NotFound();
            }
            _context.Osobe.Remove(osoba);
            _context.SaveChanges();

            return Ok("Osoba je izbrisana iz baze podataka");
        }

        // akcija koja pretrazuje entitet po imenu osobe
        [HttpGet("{ime}")]
        public IActionResult PretragaPoImenuOsobe(string ime)
        {
            var osoba = from n in _context.Osobe
                where n.Ime == ime
                select new OsobaDTO {Ime = n.Ime, Prezime = n.Prezime, Kancelarija = n.Kancelarija.Opis};
            if (osoba == null)
            {
                return NotFound();
            }
            return Ok(osoba.ToList());
        }

        // akcija koja pretrazuje entitet po imenu osobe
        [HttpGet("{opisKancelarije}")]
        public IActionResult PretragaPoOpisuKancelarije(string opisKancelarije)
        {
            var osoba = from n in _context.Osobe
                where n.Kancelarija.Opis == opisKancelarije
                select new OsobaDTO {Ime = n.Ime, Prezime = n.Prezime, Kancelarija = n.Kancelarija.Opis};
            if (osoba == null)
            {
                return NotFound();
            }
            return Ok(osoba.ToList());
        }

        // akcija koja pretrazuje entitet po datumu od/do i vraca uredjaja i osobu 
        [HttpGet("{odDatum}/{doDatum}")]
        public IActionResult PretragaPoDatumuOdDo(DateTime odDatum, DateTime doDatum)
        {
            var bzz = from n in _context.OsobaUredjaj
                      where n.PocetakKoriscenja == odDatum && n.KrajKoriscenja == doDatum
                      select new { Osoba = n.Osoba.Ime, Uredjaj = n.Uredjaj.Naziv };

            return Ok(bzz.ToList());
        }
    }
}