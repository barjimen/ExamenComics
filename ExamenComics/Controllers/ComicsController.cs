using Microsoft.AspNetCore.Mvc;
using ExamenComics.Models;
using ExamenComics.Repositories;

namespace ExamenComics.Controllers
{
    public class ComicsController : Controller
    {
        RepositoryComics repo;

        public ComicsController()
        {
            repo = new RepositoryComics();
        }

        public IActionResult Index()
        {
            List<Comics> comics = this.repo.GetComics();
            return View(comics);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Comics comics)
        {
            await this.repo.InsertComic(comics.Nombre, comics.Imagen, comics.Descripcion);
            return RedirectToAction("Index");
        }

        //Para el buscador

        public IActionResult Buscador()
        {
            ViewData["NOMBRE"] = this.repo.GetNombres();     
            return View();
        }
        [HttpPost]
        public IActionResult Buscador(string nombre)
        {
            List<Comics> comic = this.repo.DetallesComics(nombre);
            ViewData["NOMBRE"] = this.repo.GetNombres();
            return View(comic);

        }
    }
}
