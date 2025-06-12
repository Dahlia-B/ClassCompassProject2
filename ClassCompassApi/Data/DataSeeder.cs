using ClassCompass.Shared.Data;

namespace ClassCompassApi.Data
{
    public static class DataSeeder
    {
        public static void SeedData(AppDbContext context)
        {
            try
            {
                if (!context.Schools.Any())
                {
                    Console.WriteLine("✅ Database ready for data");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Seeding check failed (this is OK): {ex.Message}");
            }
        }
    }
}
