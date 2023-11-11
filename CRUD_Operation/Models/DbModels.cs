using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CRUD_Operation.Models
{
    public enum Grade
    {
        G01,
        G02,
        G03,
        G04
    }
    public class Sports
    {
        public int SportsId { get; set; }
        [Required, StringLength(50, ErrorMessage = "Sports Name is required!"), Display(Name = "Sports Name")]
        public string SportsName { get; set; }
        // nev
        public virtual ICollection<Player> Players { get; set; }
    }
    public class Player
    {
        [Display(Name = "Player Id")]
        public int PlayerId { get; set; }
        [Required, Display(Name = "Player Name")]
        public string PlayerName { get; set; }
        [Required, Column(TypeName = "date"), Display(Name = "Join Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime JoinDate { get; set; }
        [EnumDataType(typeof(Grade)), Display(Name = "Player Grade")]
        public Grade? PlayerGrade { get; set; }
        public string PicturePath { get; set; }
        // FK
        [ForeignKey("Sports")]
        public int SportsId { get; set; }
        // nev
        public virtual Sports Sports { get; set; }
    }
    public class ClubDbContext : DbContext
    {
        public ClubDbContext()
        {
            Database.SetInitializer(new DbInitializer());
        }
        public DbSet<Sports> Sports { get; set; }
        public DbSet<Player> Players { get; set; }
    }
    public class DbInitializer : DropCreateDatabaseIfModelChanges<ClubDbContext>
    {
        protected override void Seed(ClubDbContext context)
        {
            context.Sports.Add(new Sports { SportsName = "Cricket" });
            context.Sports.Add(new Sports { SportsName = "Fooball" });
            context.Sports.Add(new Sports { SportsName = "Hockey" });

            context.Players.Add(new Player
            {
                PlayerName = "Shakib al Hasan",
                JoinDate = DateTime.Now.AddYears(-18),
                PlayerGrade = Grade.G01,
                PicturePath = "~/Images/abc.png"
            });

            context.Players.Add(new Player
            {
                PlayerName = "Tamim Iqbal",
                JoinDate = DateTime.Now.AddYears(-16),
                PlayerGrade = Grade.G01,
                PicturePath = "~/Images/abc.png"
            });

            context.Players.Add(new Player
            {
                PlayerName = "Mahamudullah Riyad",
                JoinDate = DateTime.Now.AddYears(-18),
                PlayerGrade = Grade.G01,
                PicturePath = "~/Images/abc.png"
            });

            context.Players.Add(new Player
            {
                PlayerName = "Lionel Messi",
                JoinDate = DateTime.Now.AddYears(-20),
                PlayerGrade = Grade.G01,
                PicturePath = "~/Images/abc.png"
            });

            context.Players.Add(new Player
            {
                PlayerName = "Cristiano Ronaldo",
                JoinDate = DateTime.Now.AddYears(-19),
                PlayerGrade = Grade.G01,
                PicturePath = "~/Images/abc.png"
            });

            context.Players.Add(new Player
            {
                PlayerName = "Neymar Jr.",
                JoinDate = DateTime.Now.AddYears(-16),
                PlayerGrade = Grade.G01,
                PicturePath = "~/Images/abc.png"
            });
            context.SaveChanges();

            base.Seed(context);
        }
    }
}