using Citizen17.FontAwesomeTagHelper;

var builder = WebApplication.CreateBuilder(args); 
builder.Services.AddRazorPages();
builder.Services.Configure<FontAwesomeOptions>(options =>
{
    options.Set = FontAwesomeSet.Brands;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
