using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOTools.Models
{
    public partial class MapTranslationModel : ObservableObject
    {
        [ObservableProperty]
        private string _nameJP;

        [ObservableProperty]
        private string _nameTranslated;
    }
}
