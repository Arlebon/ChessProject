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
    }

}
