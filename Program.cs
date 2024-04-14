using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using projectwerk.Data;
using projectwerk.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

/*
using ProjectwerkContext context = new ProjectwerkContext();

OrderDetail item = new OrderDetail()
{
    Quantity = 1,
    Name = "soep van de dag",
    Price = 1.10M,

};
context.OrderDetails.Add(item);
context.SaveChanges();
/*
Product SoepVanDeDag = new Product()
{
    Name = "soep van de dag",
    Price = 1.10M,
    ImageUrl = "/images/soep.jpg" // Example image URL
};
context.Products.Add(SoepVanDeDag);

Product StukStokbrood = new Product()
{
    Name = "stuk stokbrood",
    Price = 0.55M,
    ImageUrl = "/images/StukStokbrood.jpg"
};
context.Products.Add(StukStokbrood);


Product Boter = new Product()
{
    Name = "boter",
    Price = 0.35M,
    ImageUrl = "/images/Boter.jpg"
};
context.Products.Add(Boter);

Product PastaGroot = new Product()
{
    Name = "pasta groot",
    Price = 5.50M,
    ImageUrl = "/images/PastaGroot.jpg"
};
context.Products.Add(PastaGroot);

Product MaaltijdSalade = new Product()
{
    Name = "maaltijdsalade",
    Price = 6.00M,
    ImageUrl = "/images/MaaltijdSalade.jpg"
};
context.Products.Add(MaaltijdSalade);

Product Panini = new Product()
{
    Name = "panini",
    Price = 4.00M,
    ImageUrl = "/images/Panini.jpg"
};
context.Products.Add(Panini);

Product KaasGoenten = new Product()
{
    Name = "kaas / Groenten",
    Price = 2.85M,
    ImageUrl = "/images/KaasGoenten.jpg"
};

context.Products.Add(KaasGoenten);

Product HamGroenten = new Product()
{
    Name = "Ham / Groenten",
    Price = 2.85M,
    ImageUrl = "/images/HamGroenten.jpg"
};

context.Products.Add(HamGroenten);

Product PrepareGroenten = new Product()
{
    Name = "Prepare / Groenten",
    Price = 2.85M,
    ImageUrl = "/images/PrepareGroenten.jpg"
};

context.Products.Add(PrepareGroenten);

Product SmosGroenten = new Product()
{
    Name = "Smos / Groenten",
    Price = 3.10M,
    ImageUrl = "/images/SmosGroenten.jpg"
};
context.Products.Add(SmosGroenten);

Product KipCurrySaladeGroenten = new Product()
{
    Name = "Kip Currysalade / Groenten",
    Price = 3.10M,
    ImageUrl = "/images/KipCurrySaladeGroenten.jpg"
};
context.Products.Add(KipCurrySaladeGroenten);

Product SurimiGroennten = new Product()
{
    Name = "Surimi / Groenten",
    Price = 3.10M,
    ImageUrl = "/images/SurimiGroennten.jpg"
};

context.Products.Add(SurimiGroennten);

Product GerookteHamEffi = new Product()
{
    Name = "Gerookte Ham / Effi",
    Price = 4.00M,
    ImageUrl = "/images/GerookteHamEffi.jpg"
};

context.Products.Add(GerookteHamEffi);

Product GerookteZalmBoursin = new Product()
{
    Name = "Gerookte Zalm / Boursin",
    Price = 4.00M,
    ImageUrl = "/images/GerookteZalmBoursin.jpg"
};


context.Products.Add(GerookteZalmBoursin);

Product StukFruit = new Product()
{
    Name = "stuk fruit",
    Price = 0.35M,
    ImageUrl = "/images/StukFruit.jpg"
};

context.Products.Add(StukFruit);

Product Yoghurt = new Product()
{
    Name = "yoghurt",
    Price = 1.30M,
    ImageUrl = "/images/Yoghurt.jpg"
};

context.Products.Add(Yoghurt);

Product HomeMadeDessert = new Product()
{
    Name = "Home made dessert",
    Price = 2.20M,
    ImageUrl = "/images/HomeMadeDessert.jpg"
};

context.Products.Add(HomeMadeDessert);

Product CrazzyBerry = new Product()
{
    Name = "Crazzy Berry",
    Price = 2.75M,
    ImageUrl = "/images/CrazzyBerry.jpg"
};

context.Products.Add(CrazzyBerry);

Product GoodFood = new Product()
{
    Name = "Good Food",
    Price = 2.75M,
    ImageUrl = "/images/GoodFood.jpg"
};

context.Products.Add(GoodFood);

Product MuffinDonut = new Product()
{
    Name = "Muffin / Donut",
    Price = 1.45M,
    ImageUrl = "/images/MuffinDonut.jpg"
};

context.Products.Add(MuffinDonut);

Product Gebak = new Product()
{
    Name = "gebak",
    Price = 1.65M,
    ImageUrl = "/images/Gebak.jpg"
};

context.Products.Add(Gebak);

Product DessertVoorverpakt = new Product()
{
    Name = "DessertVoorverpakt",
    Price = 1.30M,
    ImageUrl = "/images/DessertVoorverpakt.jpg"
};

context.Products.Add(DessertVoorverpakt);


Product Snoep = new Product()
{
    Name = "snoep",
    Price = 1.30M,
    ImageUrl = "/images/Snoep.jpg"
};

context.Products.Add(Snoep);


Product KinderBueno = new Product()
{
    Name = "Kinder Bueno",
    Price = 1.45M,
    ImageUrl = "/images/KinderBueno.jpg"
};

context.Products.Add(KinderBueno);

Product OxfamChipsChocolade = new Product()
{
    Name = "Oxfam Chips / Chocolade",
    Price = 1.65M,
    ImageUrl = "/images/OxfamChipsChocolade.jpg"
};


context.Products.Add(OxfamChipsChocolade);

Product InnocentSmoothie = new Product()
{
    Name = "Innocent Smoothie",
    Price = 3.10M,
    ImageUrl = "/images/InnocentSmoothie.jpg"
};

context.Products.Add(InnocentSmoothie);

Product PetWater = new Product()
{
    Name = "pet water (0,50L)",
    Price = 1.30M,
    ImageUrl = "/images/PetWater.jpg"
};

context.Products.Add(PetWater);

Product PetFrisdrank = new Product()
{
    Name = "Pet Frisdrank (0,50L)",
    Price = 1.75M,
    ImageUrl = "/images/PetFrisdrank.jpg"
};

context.Products.Add(PetFrisdrank);

Product PetVruchtensap = new Product()
{
    Name = "Pet Vruchtensap (0,35L)",
    Price = 1.75M,
    ImageUrl = "/images/PetVruchtensap.jpg"
};

context.Products.Add(PetVruchtensap);

Product BrikCecemelAlpro = new Product()
{
    Name = "Brik Cecemel / Alpro",
    Price = 1.75M,
    ImageUrl = "/images/BrikCecemelAlpro.jpg"
};

context.Products.Add(BrikCecemelAlpro);

Product BlikNalu = new Product()
{
    Name = "Blik Nalu (0,25L)",
    Price = 2.20M,
    ImageUrl = "/images/BlikNalu.jpg"
};

context.Products.Add(BlikNalu);

Product PetIceTea = new Product()
{
    Name = "Pet Ice Tea (0,50L)",
    Price = 2.75M,
    ImageUrl = "/images/PetIceTea.jpg"
};

context.Products.Add(PetIceTea);

Product PetArizonaThee = new Product()
{
    Name = "Pet Arizona Thee (0,50L)",
    Price = 2.75M,
    ImageUrl = "/images/PetArizonaThee"
};

context.Products.Add(PetArizonaThee);

Product BlikRedBull = new Product()
{
    Name = "Pet Ice Tea (0,25L)",
    Price = 2.75M,
    ImageUrl = "/images/BlikRedBull.jpg"
};

context.Products.Add(BlikRedBull);
context.SaveChanges();
*/

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
