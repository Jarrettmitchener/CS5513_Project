using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebUI.Models;

using AlgorithmLibrary;
using System.Reflection.Metadata.Ecma335;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //above is just pregenerated code, below is custom made
        //Don't worry about understanding everything here as this
        //isn't the important part of our project and is just the UI

        //view model (what the the view returns to the controller (this file).
        //this exchange can go either way, ie view to controller and controller to view)
        public class SearchViewModel
        {
            public string Keywords { get; set; }
            public int Algorithm { get; set; }
            public bool Dataset1Selected { get; set; }
            public bool Dataset2Selected { get; set; }
            public bool Dataset3Selected { get; set; }
            public bool Dataset4Selected { get; set; }
            public bool Dataset5Selected { get; set; }
            public bool Advanced { get; set; }
            public int q_gramValue { get; set; }
        }

        //gets the model data from the submitted
        [HttpPost]
        public IActionResult ProcessSearch(SearchViewModel model)
        {
            //makes a message to return to the web page (this doesn't matter in the long run just used to show we are returning something)
            var message = $"Search keywords: {model.Keywords}, Algorithm: {model.Algorithm}";
            //assings the message to the view under "SearchMessage"
            ViewData["SearchMessage"] = message;

            //Perform logic based on algorithm chosen:

            //driver file from our class library "AlgorithmLibrary"
            AlgoDriver algDriver = new AlgoDriver();
            string retunString = "";

            //sets up query parameters
            QueryParameters queryParameters = new QueryParameters
            {
                //assigns the datasets
                Dataset1 = model.Dataset1Selected,
                Dataset2 = model.Dataset2Selected,
                Dataset3 = model.Dataset3Selected,
                Dataset4 = model.Dataset4Selected,
                Dataset5 = model.Dataset5Selected,
                keywords = new List<string>(model.Keywords.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            };

            //separates our input based on which algorithm the user chose
            //Should be no algorithm logic here just selection
            switch (model.Algorithm)
            {
                case 1:
                    singleQueryResult top_k_newResult = new singleQueryResult();
                    top_k_newResult = algDriver.TopKAlgorithm(queryParameters, model.q_gramValue);
                    return View("SingleResults", top_k_newResult);
                case 2:
                    //Need advance option or a normal search?
                    singleQueryResult result = new singleQueryResult();
                    if(model.Advanced)
                    {
                        result = algDriver.KMPAlgorithmAdvanced(queryParameters);
                    }
                    else
                    {
                        result = algDriver.KMPAlgorithm(queryParameters);
                    }
                    result.foundResults = result.foundResults.Take(20).ToList();
                    return View("SingleResults", result);
                case 3:
                    //if case if wanting advanced or not
                    singleQueryResult newResult = new singleQueryResult();
                    if(model.Advanced)
                    {
                        newResult = algDriver.BoyerMooreAdvancedAlgorithm(queryParameters);
                    }
                    else
                    {
                        newResult = algDriver.BoyerMooreAlgorithm(queryParameters);
                    }

                    //trims the results to be top 20
                    newResult.foundResults = newResult.foundResults.Take(20).ToList();
                    //for(int i = 0; i < newResult.foundResults.Count;)
                    //{
                    //    if (newResult.foundResults[i].text.Count() > )
                    //}
                    //break;

                    return View("SingleResults", newResult);

            }
            ViewData["AlgorithMessage"] = retunString;


            //directs the web app to the "SearchResults.cshtml" page
            return View("SearchResults");
        }

        

        [HttpGet]
        public IActionResult StatSearch()
        {
            return View();
        }

        public IActionResult ProcessStatSearch(StatSearchVM vm)
        {
            AlgoDriver algDriver = new AlgoDriver();
            List<comparativeQueryResult> results = algDriver.StatisticalAnalysis(vm);
            return View("StatSearchResults", results);
        }
    }
}
