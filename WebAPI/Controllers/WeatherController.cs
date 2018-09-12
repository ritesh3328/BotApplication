using Google.Apis.Dialogflow.v2.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
using System.Data.SqlClient;
using System.Data;

namespace WebAPI.Controllers
{
    public class WeatherController : ApiController
    {
        /// <summary>
        /// Post api/Product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 

        private static SqlConnection con;
        private static void connection()
        {
            string constring = "Data Source=conduentcti.database.windows.net,1433;Initial Catalog=CTICustomerDb;User Id=conduentadmin;Password=Jimmer01;MultipleActiveResultSets=true";
            con = new SqlConnection(constring);
        }

        [HttpPost]
        public Response Post(Request request)
        {
            string responseText = "Default response";

            if (request.QueryResult != null)
            {
                //Check the Intent Name
                switch (request.QueryResult.Intent.DisplayName.ToLower())
                {
                    case "checkweather":
                        var location = request.QueryResult.Parameters["geo-city"].ToString();
                        responseText = GetTemperature(location);
                        break;
                    case "outstandingbalance":
                        responseText = GetOutStandingAmount();
                        break;
                    case "bookmovie":
                        var movienames = "Sure, Please find the latest movies: Incredibles 2, Sherlock Gnomes, Peter-rabbit, Isle of Dogs, .";
                        responseText = movienames;
                        break;
                }
            }

            return new Response() { fulfillmentText = responseText, source = $"API.AI" };
        }

        public static string GetOutStandingAmount()
        {
            connection();
            // Using the 
            SqlCommand cmd = new SqlCommand("select sum(Amount) as OutstandingBalance  FROM TopUpDetails where  CustomerID=1", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();
            var data = dt.Rows[0];
            var amount = "Your Outstanding Balance is : ₹ " + data["OutstandingBalance"].ToString();
            return amount;
        }

        /// <summary>
        /// Method to get Temparature by city
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public static string GetTemperature(string city)
        {
            string temp = string.Empty;
            if (city == null)
            {
                return "City missing (but called the api)";
            }
            switch (city.ToLower())
            {
                case "washington":
                    temp = "36.5 degree";
                    break;
                case "delhi":
                    temp = "38.7 degree";
                    break;
                case "bangalore":
                    temp = "21.11 degree";
                    break;
                default:
                    temp = "0 degree";
                    break;
                case "hyderabad":
                    temp = "36.9 degree";
                    break;
            }

            return "Temperature in " + city + " is " + temp;
        }
    }
}
