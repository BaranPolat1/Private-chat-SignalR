Proje altyapısı, bir web sitesinde kullanıcıların tam anlamıyla birbirleri ile canlı ve özel mesajlaşmasını sağlamak için tasarlanmıştır. Kolayca entegre edilebilir.
Gerekli olan Script dosyaları için : https://docs.microsoft.com/tr-tr/aspnet/core/tutorials/signalr?view=aspnetcore-3.1&tabs=visual-studio
Nuget Pacgake Manager vasıtasıyla Microsoft.AspNet.SignalR.Core paketi yüklenmelidir.
Projede Lazy Loading kullanılmıştır. Eager Loading ile çalışacak olanlar 6. Adımı kendilerine göre düzenlemelidir.


1.Models klasörünün altına ORM adında klasör açılır:
            1.1.ORM klasörünün altına Entity ve Context adından 2 klasör açılır.
            1.2 Entity klasörüne "User","Message","ChatRoom","ChatRoomUser" isminde 4 sınıf eklenir ve bu sınıflar arasındaki bağlantılar kurulur.
            1.3 Context klasörüne "ProjectContext" adında sınıf eklenir ve Database connection işlemleri yapılır.
            
2.Solution'a "Hubs" adında klasör eklenir.
            2.1 Bu klasör altına "ChatHub" adında sınıf eklenir ve bu sınıf "Hub" sınıfından miras alır.
              
         
3.Startup.cs sınıfında şu değişiklikler yapılır:
           3.1. 
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
          
          3.2
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
       
4.Package Manager Console açılır ve sırasıyla şu işlemler yapılır:
                 4.1 Add-Migration InitialCreate
                 4.2 update-database
        
5. Models klasörünğn altına "VM" adında klasör açılır:
                  5.1 "VM" klasörünün altına RegisterVM ve LoginVM adında 2 sınıf eklenir.
                  5.2 RegisterVM sınıfında kullanıcıların kayıt olması için istenilen propertyler yazılır.
                  5.3 LoginVM sınıfında kullanıcıların login olması içim gerkeli propertyler yazılır.
         
6.Controllers klasörüne AuthController adında controller eklenir:
                  6.1 Register ve Login işlemlerini yapacak metodlar yazılır.
                  6.2 Register ve Login sayfları oluşturulur.
                  
7.Controllers klasörüne MessageController adında controller eklenir:
                  7.1 Kullanıcıların mesajlarını gösterme işlemini yapacak olan "ShowChatRoom" metodu yazılır.
                  7.2 "ShowChatRoom"un üzerine sağ tıklanır sırasıyla Add View > Razor View varsa LayoutPage eklenir ve sayfa oluşturulur.
 
8.Hub sınıfına kullanıcıların anlık mesajlaşmasını sağlayacak olan "Send" metodu yazılır. 

                  
9.Views > Message > ShowChatRoom sayfası açılır:
                  9.1 Sayfanın modeline "ChatRoom" sınıfı referans verilir.
                  9.2 Mesajlaşma sayfasının tasarımı yapılır.
                  9.3 Sayfaya "signalr.js" ve "chat.js" script dosyaları referans verilir.
                  9.4 "chat.js" script dosyası sayfa tasarımına ve ChatHub sınıfına göre yeniden yapılandırılır.
                  
      
                  
               

