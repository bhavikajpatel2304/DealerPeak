namespace DealerPeak.Models
{
    public class Check
    {
        public static string EmailExists(DealerPeakDbContext context, string email)
        {
            string msg = "";
            if (!string.IsNullOrEmpty(email))
            {
                var customer = context.Users.FirstOrDefault(c => c.Email.ToLower() == email.ToLower());
                if (customer != null)
                    msg = $"Entered Email Address {email} already in use.";
            }
            return msg;
        }

        public static string ContactExists(DealerPeakDbContext context, string contact)
        {
            string msg = "";
            if (!string.IsNullOrEmpty(contact))
            {
                var customer = context.Users.FirstOrDefault(c => c.Contact.Trim() == contact.Trim());
                if (customer != null)
                    msg = $"Entered Contact Number {contact} already in use.";
            }
            return msg;
        }

        public static string DealerEmailExists(DealerPeakDbContext context, string email)
        {
            string msg = "";
            if (!string.IsNullOrEmpty(email))
            {
                var dealer = context.Dealers.FirstOrDefault(c => c.DealerEmail.ToLower() == email.ToLower());
                if (dealer != null)
                    msg = $"Entered Email Address {email} already in use.";
            }
            return msg;
        }

        public static string DealerContactExists(DealerPeakDbContext context, string contact)
        {
            string msg = "";
            if (!string.IsNullOrEmpty(contact))
            {
                var dealer = context.Dealers.FirstOrDefault(c => c.DealerContact.Trim() == contact.Trim());
                if (dealer != null)
                    msg = $"Entered Contact Number {contact} already in use.";
            }
            return msg;
        }

        public static string VINExists(DealerPeakDbContext context, string VIN)
        {
            string msg = "";
            if (!string.IsNullOrEmpty(VIN))
            {
                var vehicle = context.Vehicles.FirstOrDefault(v => v.Vin.Trim().ToLower() == VIN.ToLower());
                if (vehicle != null)
                    msg = $"Entered VIN Number {VIN} already in use.";
            }
            return msg;
        }
    }
}
