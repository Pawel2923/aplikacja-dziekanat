using System;
using System.Collections.Generic;
using System.Text;

namespace aplikacja_dziekanat.pages
{
    public class Zajecia
    {
        
            public string Data { get; set; }
            public string Godzina { get; set; }
            public string Przedmiot { get; set; }
            public string Sala { get; set; }
            public string DisplayGodzina => $"Godzina: {Godzina}";
        public string DisplayData => $"Data: {Data}";
        public string DisplayPrzedmiot => $"Przedmiot: {Przedmiot}";
        public string DisplaySala => $"Sala: {Sala}";
    }
}
