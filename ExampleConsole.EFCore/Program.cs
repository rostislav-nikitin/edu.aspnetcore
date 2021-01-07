using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ExampleConsole.EFCore
{
    class Program
    {

        static async Task Main(string[] args)
        {

            Operations operations = new Operations();

            var savedStationEntry = await operations.AddStation(new Station{ Name="NY, 5 Ave" });

            var station = await operations.GetStation(savedStationEntry.StationId);

            Console.WriteLine($"Station: StationId={station.StationId}, Name={station.Name}");
        }
    }

    public class Station
    {
        [Key]
        public int StationId {get; set;}
        public string Name {get; set;}
    }

    public class Context : DbContext
    {
        public  DbSet<Station> Stations {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=127.0.0.1;Database=my_db;Username=postgres;");

        //protected override 
    }
}
