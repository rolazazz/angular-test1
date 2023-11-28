using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Test1.Models;

namespace Test1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActivitiesController : ControllerBase
    {
        private readonly ILogger<ActivitiesController> _logger;

        private Activity[] activities;




        public ActivitiesController(ILogger<ActivitiesController> logger)
        {
            _logger = logger;
            Init();
        }

        private void Init()
        {
            var marsRover = new Project { Id = 1, Name = "Mars Rover" };
            var manhattan = new Project { Id = 2, Name = "Manhattan" };

            activities = new Activity[] {
            new Activity() { Project = marsRover, Employee = new Employee{ Id=1, Name="Mario" }, Date=new DateTime(2021,08, 26),Hours=5 },
            new Activity() { Project = manhattan, Employee = new Employee{ Id=2, Name="Giovanni" }, Date=new DateTime(2021,08, 30),Hours=3 },
            new Activity() { Project = marsRover, Employee = new Employee{ Id=1, Name="Mario" }, Date=new DateTime(2021,08, 31), Hours=3 },
           };
        }


        [HttpGet]
        public IEnumerable<Activity> Get([FromQuery] string? aggregations = null)
        {
            if (!string.IsNullOrWhiteSpace(aggregations))
            {
                var items = activities
                // dynamic aggregation using an anonymous type
                .GroupBy(x => new {
                    Project = aggregations.Contains("project") ? x.Project : null,
                    Employee = aggregations.Contains("employee") ? x.Employee : null,
                    Date = aggregations.Contains("date") ? x.Date : null
                }, x => x.Hours)
                .Select(i => new Activity
                {
                    Project = i.Key.Project,
                    Employee = i.Key.Employee,
                    Date = i.Key.Date,
                    Hours = i.Sum()
                }).ToArray();

                return items;
            }
            else
            {
                return activities; 
            }

        }
    }

    internal class ActivityEqualityComparer : IEqualityComparer<Activity>
    {
        public bool Equals(Activity? x, Activity? y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] Activity obj)
        {
            throw new NotImplementedException();
        }
    }


}
