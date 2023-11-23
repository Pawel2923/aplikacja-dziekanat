using System.Collections.Generic;

namespace aplikacja_dziekanat
{
    class SelectModel 
    {
        public List<string> Classes { get; private set; }

        public SelectModel()
        {
            Classes = new List<string>
            {
                "til-s-1-1",
                "it-s-2-1",
                "it-n-2-2",
                "mt-s-1-1"
            };
        }
    };
}