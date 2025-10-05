using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessProject.DL.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int MinAge { get; set; }

        public int MaxAge { get; set; }
    }
}
