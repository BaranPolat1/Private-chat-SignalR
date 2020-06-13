Proje altyapýsý, bir web sitesinde kullanýcýlarýn tam anlamýyla birbirleri ile canlý ve özel mesajlaþmasýný saðlamak için tasarlanmýþtýr. Kolayca entegre edilebilir.
Gerekli olan Script dosyalarý için : https://docs.microsoft.com/tr-tr/aspnet/core/tutorials/signalr?view=aspnetcore-3.1&tabs=visual-studio
Nuget Pacgake Manager vasýtasýyla Microsoft.AspNet.SignalR.Core paketi yüklenmelidir.
Projede Lazy Loading kullanýlmýþtýr. Eager Loading ile çalýþacak olanlar 6. Adýmý kendilerine göre düzenlemelidir.


1.Models klasörünün altýna ORM adýnda klasör açýlýr:
            1.1.ORM klasörünün altýna Entity ve Context adýndan 2 klasör açýlýr.
            1.2 Entity klasörüne "User","Message","ChatRoom","ChatRoomUser" isminde 4 sýnýf eklenir ve bu sýnýflar arasýndaki baðlantýlar kurulur.
            1.3 Context klasörüne "ProjectContext" adýnda sýnýf eklenir ve Database connection iþlemleri yapýlýr.
         
2.Startup.cs sýnýfýnda þu deðiþiklikler yapýlýr:
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
       
3.Package Manager Console açýlýr ve sýrasýyla þu iþlemler yapýlýr:
                  3.1 Add-Migration InitialCreate
                  3.2 update-database
        
4. Models klasörünðn altýna "VM" adýnda klasör açýlýr:
                  4.1 "VM" klasörünün altýna RegisterVM ve LoginVM adýnda 2 sýnýf eklenir.
                  4.2 RegisterVM sýnýfýnda kullanýcýlarýn kayýt olmasý için istenilen propertyler yazýlýr.
                  4.3 LoginVM sýnýfýnda kullanýcýlarýn login olmasý içim gerkeli propertyler yazýlýr.
         
5.Controllers klasörüne AuthController adýnda controller eklenir:
                  5.1 Register ve Login iþlemlerini yapacak metodlar yazýlýr.
                  5.2 Register ve Login sayflarý oluþturulur.
                  
6.Controllers klasörüne MessageController adýnda controller eklenir:
                  6.1 Kullanýcýlarýn mesajlarýný gösterme iþlemini yapacak olan "ShowChatRoom" metodu yazýlýr.
                  6.2 "ShowChatRoom"un üzerine sað týklanýr sýrasýyla Add View > Razor View varsa LayoutPage eklenir ve sayfa oluþturulur.
         
7.Solution'a "Hubs" adýnda klasör eklenir.
                  7.1 Bu klasör altýna "ChatHub" adýnda sýnýf eklenir ve bu sýnýf "Hub" sýnýfýndan miras alýr.
                  7.2 Bu sýnýfta kullanýcýlarýn anlýk mesajlaþmasýný saðlayacak olan "Send" metodu yazýlýr.
                  
8.Views > Message > ShowChatRoom sayfasý açýlýr:
                  8.1 Sayfanýn modeline "ChatRoom" sýnýfý referans verilir.
                  8.2 Mesajlaþma sayfasýnýn tasarýmý yapýlýr.
                  8.3 Sayfaya "signalr.js" ve "chat.js" script dosyalarý referans verilir.
                  8.4 "chat.js" script dosyasý sayfa tasarýmýna ve ChatHub sýnýfýna göre yeniden yapýlandýrýlýr.
                  
      
                  
               

