using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Remotion.Linq.Clauses;
using ZadatakNeki.DTO;
using ZadatakNeki.Models;

namespace ZadatakNeki.Controllers
{
    [Route("api/[controller]/[action]")]
    public class OsobaUredjajController : Controller
    {
        private readonly ToDoContext _context;

        public OsobaUredjajController(ToDoContext context)
        {
            _context = context;
        }

        // svi entiteti iz bazu
        [HttpGet]
        public IActionResult Sve()
        {
            var sve = from nn in _context.OsobaUredjaj
                select new
                {
                    DatimOd = nn.PocetakKoriscenja, DatumDo = nn.KrajKoriscenja,
                    Osoba = nn.Osoba.Ime, nn.Osoba.Prezime,
                    Uredjaj = nn.Uredjaj.Naziv
                };
            return Ok(sve.ToList());
        }

        // akcija vraca entitet koji ima dati ID
        [HttpGet("{id}")]
        public IActionResult PoId(long id)
        {
            var ed = from nn in _context.OsobaUredjaj
                where nn.Id == id
                select new
                {
                    DatimOd = nn.PocetakKoriscenja,
                    DatumDo = nn.KrajKoriscenja,
                    ImeOsobe = nn.Osoba.Ime,
                    PrezimeOsobe = nn.Osoba.Prezime,
                    Uredjaj = nn.Uredjaj.Naziv
                };
            if (ed == null)
            {
                return NotFound("A nema");
            }

            return Ok(ed.ToList());
        }

        // Upisivanje novog entiteta u bazy
        [HttpPost]
        public IActionResult DodatiNovi(OsobaUredjajDTO ouNovi)
        {
            if (ouNovi == null)
            {
                return Ok("aha ne moze to tako e");
            }

            OsobaUredjaj osobaUredjaj = new OsobaUredjaj()
            {
                PocetakKoriscenja = ouNovi.PocetakKoriscenja,
                KrajKoriscenja = ouNovi.KrajKoriscenja
            };
            Kancelarija kancelarija = new Kancelarija() { Opis = ouNovi.Kancelarija };
            Osoba osoba = new Osoba()
            {
                Ime = ouNovi.Ime,
                Prezime = ouNovi.Prezime,
                Kancelarija = kancelarija
             };
            Uredjaj uredjaj = new Uredjaj() {Naziv = ouNovi.NazivUredjaja};

            osobaUredjaj.Osoba = osoba;
            osobaUredjaj.Uredjaj = uredjaj;

            _context.OsobaUredjaj.Add(osobaUredjaj);
            _context.SaveChanges();

            return Ok("Sacuvano je ;)");
        }
        
        // akcija koja menja postojeci entitet koji ima dati ID
        [HttpPut("{id}")]
        public IActionResult MenjanjeEntiteta(long id, OsobaUredjajDTO novi)
        {
            OsobaUredjaj osobaUredjaj = _context.OsobaUredjaj.Find(id);

            Kancelarija kancelarija = new Kancelarija() {Opis = novi.Kancelarija};
            Osoba osoba = new Osoba()
            {
                Ime = novi.Ime,
                Prezime = novi.Prezime,
                Kancelarija = kancelarija
            };
            Uredjaj uredjaj = new Uredjaj() {Naziv = novi.NazivUredjaja};

            osobaUredjaj.PocetakKoriscenja = novi.PocetakKoriscenja;
            osobaUredjaj.KrajKoriscenja = novi.KrajKoriscenja;
            osobaUredjaj.Osoba = osoba;
            osobaUredjaj.Uredjaj = uredjaj;
            _context.SaveChanges();

            return Ok("Zamenjemo.");
        }

        //akcija koja brise entitet iz baze
        [HttpDelete("{id}")]
        public IActionResult BrisanjeEntiteta(long id)
        {
            OsobaUredjaj gotovSi = _context.OsobaUredjaj.Find(id);

            if (gotovSi == null)
            {
                return NotFound();
            }

            _context.OsobaUredjaj.Remove(gotovSi);
            _context.SaveChanges();
            return Ok("Izbrisali ste podatak iz baze.");
        }

        // akcija koja vraca entitete koji imaju pocetakKoriscenja veci i jednak od unetog
        [HttpGet("pocetak")]
        public IActionResult PretragaPoPocetku(DateTime pocetak)
        {
            var niz = from nn in _context.OsobaUredjaj
                where nn.PocetakKoriscenja >= pocetak
                select new
                {
                    DatimOd = nn.PocetakKoriscenja,
                    DatumDo = nn.KrajKoriscenja,
                    ImeOsobe = nn.Osoba.Ime,
                    PrezimeOsobe = nn.Osoba.Prezime,
                    Uredjaj = nn.Uredjaj.Naziv
                };

            return Ok(niz.ToList());
        }

        // akcija koja vraca entitet koji ima dat pocetak i kraj koriscenja
        [HttpGet("{pocetak}/{kraj}")]
        public IActionResult PretragaPocetakKraj(DateTime pocetak, DateTime kraj)
        {
            var niz = from nn in _context.OsobaUredjaj
                where nn.PocetakKoriscenja == pocetak && nn.KrajKoriscenja == kraj
                select new
                {
                    DatimOd = nn.PocetakKoriscenja,
                    DatumDo = nn.KrajKoriscenja,
                    ImeOsobe = nn.Osoba.Ime,
                    PrezimeOsobe = nn.Osoba.Prezime,
                    Uredjaj = nn.Uredjaj.Naziv
                };

            return Ok(niz.ToList());
        }
    }
}