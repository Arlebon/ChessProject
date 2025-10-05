using ChessProject.BLL.Services;
using ChessProject.DL.Entities;
using ChessProject.DL.Enums;
using ChessProject.Extensions;
using ChessProject.Mappers;
using ChessProject.Models.Tournament;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChessProject.Controllers
{
    public class TournamentController : Controller
    {
        private readonly TournamentService _tournamentService;
        private readonly CategoryService _categoryService;
        public TournamentController(TournamentService tournamentService, CategoryService categoryService)
        {
            _tournamentService = tournamentService;
            _categoryService = categoryService;
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
            TournamentFormDto form = new TournamentFormDto();

            form.AvailableCategories = _categoryService.GetAllCategories();

            return View(form);
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

        [HttpPost]
        public IActionResult StartTournament([FromRoute] int id)
        {
            try
            {

                _tournamentService.StartTournament(id);
                return RedirectToAction("ListTournament");
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
                return RedirectToAction("ListTournament");
            }
        }

        [HttpGet("/Tournament/Details/{id}")]
        public IActionResult Details([FromRoute] int id)
        {
            try
            {
                Tournament? tournament = _tournamentService.GetOneById(id);
                return View(tournament.ToTournamentDetailsDTO());
            }
            catch
            {
                return RedirectToAction("ListTournament", "Tournament");
            }
        }

        [HttpPost("/Tournament/Enter/{id}")]
        [Authorize]
        public IActionResult EnterTournament([FromRoute] int id)
        {
            try
            {
                _tournamentService.AddUserToTournament(id, User.GetId());

                return RedirectToAction("Details", "Tournament", new { id });
            }
            catch (Exception ex)
            {
                TempData["RegistrationError"] = ex.Message;

                return RedirectToAction("Details", "Tournament", new { id });
            }
        }
    }

}
