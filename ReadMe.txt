Proje altyap�s�, bir web sitesinde kullan�c�lar�n tam anlam�yla birbirleri ile canl� ve �zel mesajla�mas�n� sa�lamak i�in tasarlanm��t�r. Kolayca entegre edilebilir.
Gerekli olan Script dosyalar� i�in : https://docs.microsoft.com/tr-tr/aspnet/core/tutorials/signalr?view=aspnetcore-3.1&tabs=visual-studio
Nuget Pacgake Manager vas�tas�yla Microsoft.AspNet.SignalR.Core paketi y�klenmelidir.
Projede Lazy Loading kullan�lm��t�r. Eager Loading ile �al��acak olanlar 6. Ad�m� kendilerine g�re d�zenlemelidir.


1.Models klas�r�n�n alt�na ORM ad�nda klas�r a��l�r:
            1.1.ORM klas�r�n�n alt�na Entity ve Context ad�ndan 2 klas�r a��l�r.
            1.2 Entity klas�r�ne "User","Message","ChatRoom","ChatRoomUser" isminde 4 s�n�f eklenir ve bu s�n�flar aras�ndaki ba�lant�lar kurulur.
            1.3 Context klas�r�ne "ProjectContext" ad�nda s�n�f eklenir ve Database connection i�lemleri yap�l�r.
         
2.Startup.cs s�n�f�nda �u de�i�iklikler yap�l�r:
            2.1. 
            public void ConfigureServices(IServiceCollection services)
             {
            services.AddControllersWithViews();
            services.AddDbContext<ProjectContext>();
            services.AddSignalR();
            services.AddDistributedMemoryCache();
            services.AddRazorPages();
            services.AddMvc();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(5000);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
         }
          
          2.2
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ProjectContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            dbContext.Database.EnsureCreated();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chatHub");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
             });
        }
       
3.Package Manager Console a��l�r ve s�ras�yla �u i�lemler yap�l�r:
                  3.1 Add-Migration InitialCreate
                  3.2 update-database
        
4. Models klas�r�n�n alt�na "VM" ad�nda klas�r a��l�r:
                  4.1 "VM" klas�r�n�n alt�na RegisterVM ve LoginVM ad�nda 2 s�n�f eklenir.
                  4.2 RegisterVM s�n�f�nda kullan�c�lar�n kay�t olmas� i�in istenilen propertyler yaz�l�r.
                  4.3 LoginVM s�n�f�nda kullan�c�lar�n login olmas� i�im gerkeli propertyler yaz�l�r.
         
5.Controllers klas�r�ne AuthController ad�nda controller eklenir:
                  5.1 Register ve Login i�lemlerini yapacak metodlar yaz�l�r.
                  5.2 Register ve Login sayflar� olu�turulur.
                  
6.Controllers klas�r�ne MessageController ad�nda controller eklenir:
                  6.1 Kullan�c�lar�n mesajlar�n� g�sterme i�lemini yapacak olan "ShowChatRoom" metodu yaz�l�r.
                  6.2 "ShowChatRoom"un �zerine sa� t�klan�r s�ras�yla Add View > Razor View varsa LayoutPage eklenir ve sayfa olu�turulur.
         
7.Solution'a "Hubs" ad�nda klas�r eklenir.
                  7.1 Bu klas�r alt�na "ChatHub" ad�nda s�n�f eklenir ve bu s�n�f "Hub" s�n�f�ndan miras al�r.
                  7.2 Bu s�n�fta kullan�c�lar�n anl�k mesajla�mas�n� sa�layacak olan "Send" metodu yaz�l�r.
                  
8.Views > Message > ShowChatRoom sayfas� a��l�r:
                  8.1 Sayfan�n modeline "ChatRoom" s�n�f� referans verilir.
                  8.2 Mesajla�ma sayfas�n�n tasar�m� yap�l�r.
                  8.3 Sayfaya "signalr.js" ve "chat.js" script dosyalar� referans verilir.
                  8.4 "chat.js" script dosyas� sayfa tasar�m�na ve ChatHub s�n�f�na g�re yeniden yap�land�r�l�r.
                  
      
                  
               

