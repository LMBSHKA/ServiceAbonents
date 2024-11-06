using ServiceAbonents.Models;

namespace ServiceAbonents.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        public static void SeedData(AppDbContext context)
        {
            if (!context.Abonents.Any())
            {
                Console.WriteLine("--> Seeding.....");
                
                context.Abonents.AddRange(
                    new Abonent()
                    {
                        Name = "Alladin",
                        Surname = "Kover",
                        Patronymic = "Alladinovich",
                        PhoneNumber = "79547883874",
                        PasportData = "1234123456",
                        TarrifId = 1,
                        Balance = 10,
                    },
                    new Abonent()
                    {
                        Name = "Abu",
                        Surname = "Makakav",
                        Patronymic = "Makakovich",
                        PhoneNumber = "799999999",
                        PasportData = "887544321",
                        TarrifId = 2,
                        Balance = 30,
                    }
                    );
                context.SaveChanges();
            }

            else
                Console.WriteLine("--> We have data");
        }
    }
}
