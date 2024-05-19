using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer.Models
{
    public class NoteBackgroundModel
    {
        public string colour { get; set; }

        public string category { get; set; }
    }
    public enum Colours { Coral, Peach, Sand, Mint, Sage, Fog, Storm, Dusk, Blossom, Clay};
    public enum Categories { Groceries, Food, Music, Recipes, Notes, Places, Travel, Video, Celebration};

}
