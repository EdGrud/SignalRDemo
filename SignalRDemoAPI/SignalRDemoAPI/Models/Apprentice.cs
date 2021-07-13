using System;

namespace SignalRDemoAPI.Models
{
    public class Apprentice
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public MaterialComprehension ComprehensionLevel { get; set; }
    }
}