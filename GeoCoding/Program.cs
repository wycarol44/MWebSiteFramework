using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapQuest2; 


    class Program
    {
        static void Main(string[] args)
        {
            //IGeocoder geocoder = new Geocoding.Google.GoogleGeocoder() { ApiKey = "AIzaSyD35Igu2SxymNNjE5LTmbfg6fmfx97ZDOg" };
            //IEnumerable<Address> addresses = geocoder.Geocode("1600 pennsylvania ave washington dc");
            //Console.WriteLine("Formatted: " + addresses.First().FormattedAddress); //Formatted: 1600 Pennslyvania Avenue Northwest, Presiden'ts Park, Washington, DC 20500, USA
            //Console.WriteLine("Coordinates: " + addresses.First().Coordinates.Latitude + ", " + addresses.First().Coordinates.Longitude); //Coordinates: 38.8978378, -77.0365123
            ////Console.ReadLine();

            //IGeocoder mapquest = new Geocoding.MapQuest.MapQuestGeocoder("Fmjtd%7Cluub2huanl%2C20%3Do5-9uzwdz") { UseOSM = true };
            //IEnumerable<Address> mapAddress = mapquest.Geocode("lancaster,PA");
            //Console.WriteLine("Formatted: " + addresses.First().FormattedAddress); //Formatted: 1600 Pennslyvania Avenue Northwest, Presiden'ts Park, Washington, DC 20500, USA
            //Console.WriteLine("Coordinates: " + addresses.First().Coordinates.Latitude + ", " + addresses.First().Coordinates.Longitude); //Coordinates: 38.8978378, -77.0365123
            //Console.ReadLine();


            MapQuest2.IGeocoder mapquest = new MapQuest2.MapQuestGeocoder("MY_APP_KEY") { UseOSM = true };
            IEnumerable<Address> addresses = mapquest.Geocode("1600 pennsylvania ave washington dc");
            Console.WriteLine("Formatted: " + addresses.First().FormattedAddress); //Formatted: 1600 Pennslyvania Avenue Northwest, Presiden'ts Park, Washington, DC 20500, USA
            Console.WriteLine("Coordinates: " + addresses.First().Coordinates.Latitude + ", " + addresses.First().Coordinates.Longitude); //Coordinates: 38.8978378, -77.0365123
            Console.ReadLine();
        }
    }

