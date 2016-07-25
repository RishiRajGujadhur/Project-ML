using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Save()
        {

            return View();
        }

        public class StringTable
        {
            public string[,] Values { get; set; }
        }

        public async Task<JsonResult> Test(QueryData c)
        {
            using (var client = new HttpClient())
            {
                 List<StringTable> all = null;
                 all = new List<StringTable>();
                #region Post
                var scoreRequest = new
                {

                    Inputs = new Dictionary<string, StringTable>() { 
                        { 
                            "input1", 
                            new StringTable() 
                            {
                                //ColumnNames = new string[] {"Pclass", "Sex", "Age", "Embarked"},
                                Values = new string[,] {  { c.Pclass.ToString(), c.Sex, c.Age.ToString(), c.Embarked }  }
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };
                const string apiKey = "zGEEXM7Ua9r4cT1/CEMm8E2S/CF2xGcrqvRD3ilEG8WTCe7QAJy0OHwBoxDXCofcexXuH5xk4V2XX3H6AkvBqw=="; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/331acdcb3e9b4bd2836d4eb003c2f372/services/721d99cb94cb4c92a7c14f1a92846649/execute?api-version=2.0&details=true");

                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);
                #endregion
               
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    //JObject o = JObject.Parse(result);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                #region else
                else
                {
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));
                    return Json(response.StatusCode, JsonRequestBehavior.AllowGet);
                    // Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
                    //Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    
                  
                    return Json(responseContent, JsonRequestBehavior.AllowGet);
                }
                return Json("hello world", JsonRequestBehavior.AllowGet);
                #endregion
            }
               
        }

      
    }
}