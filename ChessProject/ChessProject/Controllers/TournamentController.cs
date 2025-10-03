using ChessProject.BLL.Services;
using ChessProject.DL.Entities;
using ChessProject.DL.Enums;
using ChessProject.Mappers;
using ChessProject.Models.Tournament;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        public IActionResult CreateTournament()
        {
            return View(new TournamentFormDto());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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

        [HttpPost]
        public IActionResult DeleteTournament([FromRoute]int id)
        {
            
            bool deleted = _tournamentService.DeleteTournamentById(id);
            if (!deleted) 
            {
                TempData["OnDeleteMessage"] = $"Tournament with Id {id} was not found.";
            }else
            {
                TempData["OnDeleteMessage"] = $"Tournament deleted succesfully.";
            }
            return RedirectToAction("ListTournament");
        }
    }

}
