using Final_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System;
using System.Xml.Linq;
using QuizletAnswerFinder.AnswerFinding;
using QuizletAnswerFinder.Models;
using QuizletAnswerFinder;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Final_Project.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Controllers
{
    public class SolverController : Controller
    {
        AnswerFinder _answerFinder;
        ApplicationDbContext dbContext;
        public SolverController(ApplicationDbContext applicationDbContext)
        {
            _answerFinder = new AnswerFinder();
            dbContext = applicationDbContext;
        }

        [HttpPost]
        public IActionResult Query(UserEnterBindingModel model)
        {


            //Check if subject already exist in the database
            SubjectModel subjectModel = dbContext.Subjects.Where(s => s.Subject.Equals(model.Subject)).Include(x => x.MatchingQuerys).FirstOrDefault();

            if (subjectModel != null)
            {

                if (subjectModel.MatchingQuerys != null)
                {
                    Query foundQueryFromSubject = subjectModel.MatchingQuerys.Where(q => q.Question.Equals(model.Question)).FirstOrDefault();

                    //Found the question. Display it.

                    if (foundQueryFromSubject != null)
                    {
                        return View(foundQueryFromSubject);
                    }
                }


            }
            else
            {
                //Save the new subject in the database
                dbContext.InsertSubject(new SubjectModel { Subject = model.Subject });
            }

            //The subject exists. The query does not.

            Query query = new Query();

            SubjectModel subjectForQuery = dbContext.Subjects.Where(s => s.Subject.Equals(model.Subject)).First();


            //match the question asked to the user logged in
            query.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //Add the subject id to the query
            //   query.SubjectModelId = subjectForQuery.ID;


            //get the response from algorithm
            List<QuestionInfo> res = _answerFinder.FindAnswers(model.Question, model.Subject);

            //Convert the response into a readable format
            StringBuilder stringBuilder = new StringBuilder();
            const String newLine = "<br>";
            for (int i = 0; i < res.Count; i++)
            {
                if (i == 1)
                {
                    query.IsMultipleAnswer = true;
                }
                stringBuilder.Append(newLine);
                stringBuilder.Append(res[i].Question);
                stringBuilder.Append(newLine);
                stringBuilder.Append(res[i].Answer);

            }

            query.Question = model.Question;
            query.Answer = stringBuilder.ToString();




            subjectForQuery.MatchingQuerys.Add(query);

            dbContext.SaveChanges();

            return View(query);

        }

    }
}