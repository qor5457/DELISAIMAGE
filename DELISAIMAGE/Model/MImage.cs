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
        public int X {get;set;}
        
        public int Y {get;set;}

        public int Width {get;set;}

        public int Height {get;set;}
        
        public  int Count { get; set; }
    }
}