using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace EOTools.Models
{
    public class LockData : ObservableObject
    {
        private int id;
        public int Id
        {
            get { return id; }
            set 
            { 
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        [JsonIgnore]
        private Color? lockColor { get; set; }

        [JsonIgnore]
        public Color? LockColor
        {
            get
            {
                if (lockColor is null)
                {
                    lockColor = Color.FromArgb(ColorA, ColorR, ColorG, ColorB);
                }

                return lockColor;
            }
            set
            {
                if (value is Color color)
                {
                    ColorA = color.A;
                    ColorR = color.R;
                    ColorG = color.G;
                    ColorB = color.B;
                }

                lockColor = value;
            }
        }

        [JsonProperty("A")]
        public byte ColorA { get; set; }


        [JsonProperty("R")]
        public byte ColorR { get; set; }


        [JsonProperty("G")]
        public byte ColorG { get; set; }


        [JsonProperty("B")]
        public byte ColorB { get; set; }

        public string Name { get; set; }
    }
}
