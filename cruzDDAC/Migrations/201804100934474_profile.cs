namespace cruzDDAC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class profile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Fullname", c => c.String());
            AddColumn("dbo.AspNetUsers", "Profile", c => c.String());
            AddColumn("dbo.AspNetUsers", "Credit", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Credit");
            DropColumn("dbo.AspNetUsers", "Profile");
            DropColumn("dbo.AspNetUsers", "Fullname");
        }
    }
}
