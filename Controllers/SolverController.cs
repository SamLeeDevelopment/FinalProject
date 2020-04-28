using Final_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Final_Project.Algorithm;
using System;
using System.Xml.Linq;

namespace Final_Project.Controllers
{
    public class SolverController : Controller
    {
        public AnswerFinder _answerFinder;

        public SolverController ()
        {
            this._answerFinder = new AnswerFinder();
        }

        [HttpPost]
        public IActionResult Query(Query model)
        {
            //match the question asked to the user logged in
            model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //get the response from algorithm (xml)
            model.Response = _answerFinder.FindAnswers(model.Question, "sql");

            //parse xml into string to be displayed
            model.Answer = model.Response.Split("<Answer>")[1].Split("</Answer>")[0];

            return View(model);
        }
    }
}