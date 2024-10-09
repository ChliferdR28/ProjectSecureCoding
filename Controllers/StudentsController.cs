using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectSecureCoding.Data;
using ProjectSecureCoding.Models;

namespace ProjectSecureCoding.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("mahasiswa")]
        public IActionResult Index()
        {
            var mahasiswa = _context.Mahasiswa.ToList();
            return View("Index", mahasiswa);
        }
        public IActionResult Tambah()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PostTambah(Mahasiswa mahasiswa)
        {
            if (ModelState.IsValid)
            {
                _context.Mahasiswa.Add(mahasiswa);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(mahasiswa);
        }
    }
}