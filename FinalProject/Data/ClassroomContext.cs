using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinalProject.Models;

namespace FinalProject.Data
{
    public class ClassroomContext : DbContext
    {
        public ClassroomContext (DbContextOptions<ClassroomContext> options)
            : base(options)
        {
        }

        public DbSet<FinalProject.Models.Classroom> Classroom { get; set; }
    }
}
