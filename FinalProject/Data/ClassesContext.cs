using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinalProject.Models;

namespace FinalProject.Data
{
    public class ClassesContext : DbContext
    {
        public ClassesContext (DbContextOptions<ClassesContext> options)
            : base(options)
        {
        }

        public DbSet<FinalProject.Models.Classes> Classes { get; set; }

        public DbSet<FinalProject.Models.Teacher> Teacher { get; set; }

        public DbSet<FinalProject.Models.Classroom> Classroom { get; set; }
    }
}
