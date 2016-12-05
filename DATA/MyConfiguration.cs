using System.Data.Entity;

namespace DATA
{
    public class MyConfiguration : DbConfiguration
    {
        public MyConfiguration()
        {
            AddInterceptor(new StringTrimmerInterceptor());
        }
    }
}
