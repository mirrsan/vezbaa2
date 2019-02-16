using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZadatakNeki.Models;

namespace ZadatakNeki.Controllers
{
    [Route("api/[controller]")]
    public class BaseController<T> : Controller
    {
        private readonly ToDoContext _context;

        public BaseController(ToDoContext context)
        {
            _context = context;
        }

      

    }
}