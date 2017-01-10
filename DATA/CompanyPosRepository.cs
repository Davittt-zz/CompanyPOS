using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA
{
    public class CompanyPosRepository
    {
        public IEnumerable<Company> GetCompanies()
        {
            CompanyPosDBContext databaseDBContext = new CompanyPosDBContext();
            return databaseDBContext.Companies.Include("Stores").ToList();
            //return databaseDBContext.Companies.ToList();
        }
    }
}
