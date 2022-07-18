using System;
using System.Collections.Generic;

namespace DELISAIMAGE.Model
{
    public class ModelImage
    { 
        public string Imagepath { get; set; }
    }

    public class BoxLocation
    {
        public string Name { get; set; }

        [NotDataTableColumn]
        public int X {get;set;}
        [NotDataTableColumn]
        public int Y {get;set;}
        [NotDataTableColumn]
        public int Width {get;set;}
        [NotDataTableColumn]
        public int Height {get;set;}
        
        public  int Count { get; set; }
    }
}