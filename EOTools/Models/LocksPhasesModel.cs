using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOTools.Models
{
    public class LocksPhasesModel
    {
        public List<LockData> Locks { get; set; } = new List<LockData>();

        public List<LockPhaseData> Phases { get; set; } = new List<LockPhaseData>();
    }
}
