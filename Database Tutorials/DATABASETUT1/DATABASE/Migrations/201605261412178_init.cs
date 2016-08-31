namespace DATABASE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Code = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Author = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        StockId = c.Int(nullable: false, identity: true),
                        OnHand = c.Int(nullable: false),
                        OnOrder = c.Int(nullable: false),
                        Item_Code = c.Int(),
                        Store_StockId = c.Int(),
                    })
                .PrimaryKey(t => t.StockId)
                .ForeignKey("dbo.Books", t => t.Item_Code)
                .ForeignKey("dbo.Stores", t => t.Store_StockId)
                .Index(t => t.Item_Code)
                .Index(t => t.Store_StockId);
            
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        StockId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.StockId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stocks", "Store_StockId", "dbo.Stores");
            DropForeignKey("dbo.Stocks", "Item_Code", "dbo.Books");
            DropIndex("dbo.Stocks", new[] { "Store_StockId" });
            DropIndex("dbo.Stocks", new[] { "Item_Code" });
            DropTable("dbo.Stores");
            DropTable("dbo.Stocks");
            DropTable("dbo.Books");
        }
    }
}
