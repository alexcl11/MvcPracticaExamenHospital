using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcPracticaExamenHospital.Models;
using MvcPracticaExamenHospital.Repository;

namespace MvcPracticaExamenHospital.Controllers
{
    public class DoctoresController : Controller
    {
        RepositoryDoctores repo;

        public DoctoresController()
        {
            this.repo = new RepositoryDoctores();
        }

        public IActionResult Index()
        {
            List<Doctor> doctores = this.repo.GetDoctores();
            return View(doctores);
        }

        public IActionResult Detalles(int id)
        {
            Doctor doctor = this.repo.GetDetallesDoctor(id);
            return View(doctor);
        }

        public IActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Insert(int id, string apellido,
            string especialidad, int salario, int hospital)
        {
            await this.repo.UpsertDoctor(id,
                hospital, apellido, especialidad, salario);
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            Doctor doctor = this.repo.GetDetallesDoctor(id);
            return View(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, string apellido,
            string especialidad, int salario, int hospital)
        {
            await this.repo.UpsertDoctor(id,
                hospital, apellido, especialidad, salario);
            return RedirectToAction("Index");
        }
    }
}
