using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA
{
    public class Util
    {
        public static int GetPinNumber(string UUID, int UserID)
        {

            Random random = new Random();
            CompanyPosDBContext database = new CompanyPosDBContext();
            var dispositives = (from disp in database.Dispositives
                                where disp.UUID.Equals(UUID) && disp.UserID.Equals(UserID)
                                 && disp.Active.Equals(true)
                                select disp).ToList();

            if (dispositives.Count > 0)
            {

                int randomNumber = 0;
                bool isValid = false;
                bool pinNoEquals = true;
                do
                {
                    randomNumber = random.Next(0, 9999);
                    foreach (var disp in dispositives)
                    {
                        if (disp.PinNumber == randomNumber)
                        {
                            pinNoEquals = false;
                        }

                    }
                    if (pinNoEquals)
                    {
                        isValid = true;
                    }
                }
                while (!isValid);

                return randomNumber;
            }
            else
            {
                int randomNumber = random.Next(0, 9999);
                return randomNumber;
            }


        }
    }
}
