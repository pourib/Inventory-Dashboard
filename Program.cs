using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
var app = builder.Build();
app.UseCors("AllowAll");

string connString = "Server=.;Database=FormProject;Integrated Security=True;TrustServerCertificate=True;";

// ۱. گرفتن لیست انبار
app.MapGet("/api/anbar", () => {     // این درگاه لیست کالاها رو برمی‌گردونه که توی انبار داریم
    var list = new List<object>();
    using var conn = new SqlConnection(connString);
    conn.Open();
    using var cmd = new SqlCommand("SELECT * FROM Anbar", conn);
    using var r = cmd.ExecuteReader(); 
    while (r.Read()) list.Add(new { id = r["KalaID"], esm = r["Nam_Kala"], tedad = r["Mojoodi"] });
    return list;
});

// ۲. درگاه فروش (استفاده از همون SP که قبلاً ساختی)
app.MapPost("/api/foroosh", (int id) => {  // این درگاه وقتی کالایی فروخته میشه صدا زده میشه و SP_Sabt_Foroosh رو اجرا میکنه
    using var conn = new SqlConnection(connString);
    conn.Open();
    using var cmd = new SqlCommand("SP_Sabt_Foroosh", conn);
    cmd.CommandType = System.Data.CommandType.StoredProcedure;
    cmd.Parameters.AddWithValue("@ID_Kala", id);
    cmd.Parameters.AddWithValue("@Tedad", 1); // هر بار ۱ عدد می‌فروشیم
    cmd.ExecuteNonQuery();
    return Results.Ok();
});

// ۴. درگاه گرفتن گزارش کل سود
app.MapGet("/api/sode-kol", () => {  // این درگاه جمع کل سود رو برمی‌گردونه
    decimal sood = 0;
    string connString = "Server=.;Database=FormProject;Integrated Security=True;TrustServerCertificate=True;";
    using var conn = new SqlConnection(connString);
    conn.Open();
    
    // همون کوئری سالمی که الان تست کردی
    string query = "SELECT SUM(f.Tedad_Kharid * a.Gheymat) FROM dbo.Foroosh f JOIN dbo.Anbar a ON f.KalaID = a.KalaID";
    using var cmd = new SqlCommand(query, conn);
    
    var result = cmd.ExecuteScalar(); // گرفتن فقط یک عدد (جمع کل)
    if (result != DBNull.Value) sood = Convert.ToDecimal(result);
    
    return new { totalProfit = sood };
});
// ۵. درگاه حذف کالا
    app.MapDelete("/api/kala/{id}", (int id) => {
        string connString = "Server=.;Database=FormProject;Integrated Security=True;TrustServerCertificate=True;";
        using var conn = new SqlConnection(connString); // ایجاد اتصال به دیتابیس
        conn.Open(); // 
        // دستور اس‌کیو‌ال برای پاک کردن کالا با آیدی مشخص
        string query = "DELETE FROM dbo.Anbar WHERE KalaID = @ID"; // استفاده از پارامتر برای جلوگیری از SQL Injection
        using var cmd = new SqlCommand(query, conn); // ایجاد دستور SQL
        cmd.Parameters.AddWithValue("@ID" , id); // اضافه کردن پارامتر آیدی به دستور
        cmd.ExecuteNonQuery(); // اجرای دستور حذف
        return Results.Ok(); // بازگرداندن پاسخ موفقیت‌آمیز
    });
    app.MapPost("/api/kala", (KalaInfo info) => {  //   استفاده از رکورد برای دریافت اطلاعات کالا
        string connString = "Server=.;Database=FormProject;Integrated Security=True;TrustServerCertificate=True;";
        using var conn = new SqlConnection(connString);
        conn.Open(); // دستور اس‌کیو‌ال برای درج کالای جدید با استفاده از پارامترها
        string query = "INSERT INTO dbo.Anbar (Nam_Kala, Mojoodi, Gheymat) VALUES (@Nam, @Mojoodi, @Gheymat)";
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@Nam", info.Nam);
        cmd.Parameters.AddWithValue("@Mojoodi", info.Mojoodi);
        cmd.Parameters.AddWithValue("@Gheymat", info.Gheymat);
        cmd.ExecuteNonQuery(); // اجرای دستور درج کالا جدید
        return Results.Ok();
    });
    app.MapGet("/api/logs", () =>{
        var List = new List<string>();
        string connString = "Server=.;Database=FormProject;Integrated Security=True;TrustServerCertificate=True;";
        using var conn = new SqlConnection(connString);
        conn.Open();
        // ترکیب جدول فروش و انبار برای پیدا کردن اسم کالای فروخته شده
        string query = "SELECT a.Nam_Kala FROM dbo.Foroosh f JOIN dbo.Anbar a ON f.KalaID = a.KalaID";
        using var cmd = new SqlCommand(query, conn);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            List.Add("یک عدد " + r["Nam_Kala"] + " فروخته شد 💸");
        }
        List.Reverse(); // برعکس کردن لیست برای نمایش آخرین فروش‌ها در بالا
        return List.Take(5); // فقط ۵ مورد آخر رو برمی‌گردانیم
        
    });  

    app.MapPut("/api/kala/{id}", (int id, KalaInfo info) => {
        string connString = "Server=.;Database=FormProject;Integrated Security=True;TrustServerCertificate=True;";
        using var conn = new SqlConnection(connString);
        conn.Open();
        // دستور آپدیت برای تغییر اسم و موجودی بر اساس آیدی
        string query = "UPDATE dbo.Anbar SET Nam_Kala = @Nam, Mojoodi = @Mojoodi WHERE KalaID = @id";
        using var cmd = new SqlCommand(query , conn);
        cmd.Parameters.AddWithValue("@Nam", info.Nam);
        cmd.Parameters.AddWithValue("@Mojoodi", info.Mojoodi);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
        return Results.Ok();
    });

app.Run();

// کلاس کمکی برای دریافت اطلاعات کالای جدید
public record KalaInfo(string Nam, int Mojoodi, int Gheymat);