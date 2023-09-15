using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinalProject.Models;

namespace FinalProject.Data
{
    public class TeacherContext : DbContext
    {
        public TeacherContext (DbContextOptions<TeacherContext> options)
            : base(options)
        {
        }

        public DbSet<FinalProject.Models.Teacher> Teacher { get; set; }
    }
}
