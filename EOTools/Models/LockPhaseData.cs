using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace EOTools.Models
{
    public class LockPhaseData : ObservableObject
    {
        public List<int> LockGroups { get; set; } = new List<int>();

        public string Name { get; set; }
    }
}
