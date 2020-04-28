using Final_Project.Models;
using HtmlAgilityPack;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Algorithm
{
    public class AnswerFinder
    {

        private static ScrapingBrowser browser = new ScrapingBrowser();
        /// <summary>
        /// Holds all found question and answers from quizlet and allows to find by question
        /// </summary>
        private Dictionary<String, QuestionInfo> Questions = new Dictionary<String, QuestionInfo>();


        public String FindAnswers(String question, String subject)
        {


            question = question.ToLower().Trim();

            AnswerFormatting answerFormattingPossible = new AnswerFormatting(question);


            // I dont think subjects are case sensetive on quizlet
            List<String> setList = GetSetUrl(subject);

            foreach (String setUrl in setList)
            {

                List<QuestionInfo> questions = GetSetInfo(setUrl, question);


                // The exact question might have been found. Use dictionary to provide constent time lookup for a correct match
                if (Questions.ContainsKey(question))
                {


                    AnswerFormatting answerFormattingCorrect = new AnswerFormatting(question);

                    answerFormattingCorrect.AddQuestionInfo(Questions[question]);

                    Questions.Clear();


                    return answerFormattingCorrect.ToString();

                }


                // If the exact question was not found, try to find other possible ones.

                foreach (QuestionInfo questionInfo in questions)
                {

                    if (questionInfo.Matches() == FoundType.POSSIBLE)
                    {

                        answerFormattingPossible.AddQuestionInfo(questionInfo);

                    }

                }



            }


            Questions.Clear();

            return answerFormattingPossible.ToString();


        }





        /// <summary>
        /// Returns all links to each set that matches the subject
        /// </summary>
        /// <param name="subject">The name to use</param>
        /// <returns></returns>
        List<String> GetSetUrl(String subject)
        {

            String subjectUrl = CreateUrlFromSuject(subject);

            WebPage page = browser.NavigateToPage(new Uri(subjectUrl));

            HtmlNode node = page.Html;


            var v = node.Descendants("a").Where(x => x.GetAttributeValue("class", "").Equals("UILink")).ToList();


            List<String> SetUrls = new List<string>();

            foreach (HtmlNode n in v)
            {

                String value = n.GetAttributeValue("href", "");

                if (value.Contains("https://"))
                {
                    SetUrls.Add(value);
                }


            }

            return SetUrls;

        }


        /// <summary>
        /// Loops over a set and stores the all question and answers. 
        /// </summary>
        /// <param name="url">The url of the set</param>
        /// <returns></returns>
        List<QuestionInfo> GetSetInfo(String url, String userEnterQuestion)
        {

            //TODO do we really need these to be sorted?
            List<QuestionInfo> setInfo = new List<QuestionInfo>();

            WebPage page = browser.NavigateToPage(new Uri(url));

            HtmlNode n = page.Html;

            String name = n.Descendants("title").FirstOrDefault().InnerHtml;

            var stuff = n.Descendants("span").Where(x => x.GetAttributeValue("class", "").Contains("TermText")).ToList();

            for (int i = 0; i < stuff.Count() - 2; i += 2)
            {

                // Make sure all leading and trailing white spaces are gone. Lowercase the entire string
                String foundQuestion = stuff[i].InnerText.ToLower().Trim();
                String answer = stuff[i + 1].InnerText;


                QuestionInfo questionInfo = new QuestionInfo(foundQuestion, answer, userEnterQuestion);

                // Add the question info to the dictionary. Use the found question as the key.
                if (!Questions.ContainsKey(foundQuestion))
                {
                    setInfo.Add(questionInfo);
                    Questions.Add(foundQuestion, questionInfo);
                }



            }

            return setInfo;

        }






        ///<summary>
        ///Creates the url that points to the collection of sets
        ///</summary>
        String CreateUrlFromSuject(String subject)
        {
            return String.Format("https://quizlet.com/subject/{0}/", subject);
        }
    }
}
