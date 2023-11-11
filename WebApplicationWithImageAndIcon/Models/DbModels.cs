 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplicationWithImageAndIcon.Models
{
    public enum Grade
    {
        G01 = 1,
        G02,
        G03,
        G04
    }
    public class Sports
    {
        // The constructor of the Sports class initializes the Players property as a new empty list. This is a good practice because it ensures that the Players property is never null and can be safely used to add player objects.
        public Sports()
        {
            this.Players = new List<Player>();
        }
        public int SportsId { get; set; }
        [Required, StringLength(50, ErrorMessage = "Sports Name is required!"), Display(Name = "Sports Name")]
        public string SportsName { get; set; }
        // navigation property (representing the players associated with that sport. It's defined as a collection of Player objects.)
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
        [ForeignKey("Sports"), Display(Name = "Sports Name")]
        public int SportsId { get; set; }
        // nev
        public virtual Sports Sports { get; set; }
    }
    public class SportsDbContext : DbContext
    {
        public SportsDbContext()
        {
            Database.SetInitializer(new DbInitializer());
        }
        // DbSet properties are used to interact with the database and represent the tables in the database.
        public DbSet<Sports> Sports { get; set; }
        public DbSet<Player> Players { get; set; }
    }
    // DbInitializer is responsible for seeding the database with initial data when the model changes.
    public class DbInitializer : DropCreateDatabaseIfModelChanges<SportsDbContext>
    {
        // the Seed method, it creates two sports like, "Cricket" and "Football," and adds players to each sport.
        protected override void Seed(SportsDbContext context)
        {
            base.Seed(context);
            Sports s1 = new Sports { SportsName = "Cricket" };
            Sports s2 = new Sports { SportsName = "Fooball" };

            s1.Players.Add(new Player
            {
                PlayerName = "Shakib al Hasan",
                JoinDate = DateTime.Now.AddYears(-18),
                PlayerGrade = Grade.G01,
                PicturePath = "~/Images/abc.png"
            });

            s1.Players.Add(new Player
            {
                PlayerName = "Tamim Iqbal",
                JoinDate = DateTime.Now.AddYears(-16),
                PlayerGrade = Grade.G01,
                PicturePath = "~/Images/abc.png"
            });

            s1.Players.Add(new Player
            {
                PlayerName = "Mahamudullah Riyad",
                JoinDate = DateTime.Now.AddYears(-18),
                PlayerGrade = Grade.G01,
                PicturePath = "~/Images/abc.png"
            });

            s2.Players.Add(new Player
            {
                PlayerName = "Lionel Messi",
                JoinDate = DateTime.Now.AddYears(-20),
                PlayerGrade = Grade.G01,
                PicturePath = "~/Images/abc.png"
            });

            s2.Players.Add(new Player
            {
                PlayerName = "Cristiano Ronaldo",
                JoinDate = DateTime.Now.AddYears(-19),
                PlayerGrade = Grade.G01,
                PicturePath = "~/Images/abc.png"
            });

            s2.Players.Add(new Player
            {
                PlayerName = "Neymar Jr.",
                JoinDate = DateTime.Now.AddYears(-16),
                PlayerGrade = Grade.G01,
                PicturePath = "~/Images/abc.png"
            });

            context.Sports.AddRange(new Sports[] { s1, s2 });
            context.SaveChanges();
        }
    }
}