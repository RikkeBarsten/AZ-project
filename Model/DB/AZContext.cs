using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AZ_project.Model.DB
{
    public partial class AZContext : DbContext
    {
        public virtual DbSet<Analyse_Antal> Analyse_Antal {get; set;}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=(LocalDB)\mssqllocaldb;Database=AZ;Integrated Security=True;MultipleActiveResultSets=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Analyse_Antal> (entity => {entity.Property(e => e.MA_nr).HasColumnName("MA - nr");});
            modelBuilder.Entity<Analyse_Antal> (entity => {entity.HasKey(e => e.MA_nr);});
            
        }

        
    }
}
