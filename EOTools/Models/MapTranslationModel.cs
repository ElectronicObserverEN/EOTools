using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOTools.Models
{
    public class MapTranslationModel : ObservableObject
    {
        public string NameJP { get; set; }

        public string NameTranslated { get; set; }
    }
}
