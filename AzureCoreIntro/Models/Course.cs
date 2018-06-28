using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureCoreIntro.Models
{
    public class Course
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        public string Title { get; set; }

        public ICollection<Module> Modules { get; set; } = new List<Module>();
    }

    public class Module
    {
        public string Title { get; set; }

        public ICollection<Clip> Clips { get; set; } = new List<Clip>();
    }

    public class Clip
    {
        public string Name { get; set; }
        public int Length { get; set; }

    }
}
