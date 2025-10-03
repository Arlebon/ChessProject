using ChessProject.BLL.Services;
using ChessProject.DL.Entities;
using ChessProject.Mappers;
using ChessProject.Models.Tournament;
using Microsoft.AspNetCore.Mvc;

namespace ChessProject.Controllers
{
    public class TournamentController : Controller
    {
        private readonly TournamentService _tournamentService;
        public TournamentController(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        [HttpGet]
        public IActionResult ListTournament()
        {
            List<TournamentListDto> tournamentListDtos = [.. _tournamentService.GetAll().Select(t => t.ToTournamentListDto())];
            return View(tournamentListDtos);
        }

        [HttpGet]
        public IActionResult CreateTournament()
        {
            return View(new TournamentFormDto());
        }

        [HttpPost]
        public IActionResult CreateTournament([FromForm] TournamentFormDto tournamentForm)
        {
            if (!ModelState.IsValid)
            {
                return View(tournamentForm);
            }
            Console.WriteLine("Categories received: " + string.Join(", ", tournamentForm.Categories));
            _tournamentService.Add(tournamentForm.ToTournament());
            return RedirectToAction("ListTournament");
        }
    }

}
