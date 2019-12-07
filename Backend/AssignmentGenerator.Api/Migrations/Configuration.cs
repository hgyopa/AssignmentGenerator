namespace AssignmentGenerator.Api.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<AssignmentGenerator.Api.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AssignmentGenerator.Api.Models.ApplicationDbContext context)
        {
            //password: admin1234
            var user = new ApplicationUser
            {
                SecurityStamp = "e63e9a9c-1b10-4185-a297-1f1c66239cad",
                Email = "admin@assignmentgenerator.hu",
                PasswordHash = "ADbkAlh5aW+osK4Hrvm57AHh4eIgDcJbD+xIw1l0OFGsYw0ZuujS9/fHnP/Scobc6g==",
                UserName = "admin@assignmentgenerator.hu"
            };

            if (context.Users.FirstOrDefault(u => u.Email == user.Email) == null)
            {
                context.Users.Add(user);
            }
        }
    }
}
