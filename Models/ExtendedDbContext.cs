using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
namespace AspNetIdentityDemo.Models
{
    public class ExtendedDbContext:IdentityDbContext
    {
        public ExtendedDbContext(string connectionString)
            :base(connectionString)
        {

        }
        public DbSet<Adresse> Adresses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var adresse = modelBuilder.Entity<Adresse>();
            adresse.ToTable("AspNetUsersAdresses");
            adresse.HasKey(a => a.Id);

            var user = modelBuilder.Entity<ExtendedUser>();
            user.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("FullNameIndex")));

            user.HasMany(u => u.Adresses)
                .WithRequired()
                .HasForeignKey(a => a.UserId);

            
                
            base.OnModelCreating(modelBuilder);
        }
    }
}