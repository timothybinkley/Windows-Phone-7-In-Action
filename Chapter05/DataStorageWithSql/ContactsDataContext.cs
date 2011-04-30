using System.Data.Linq;

namespace DataStorage
{
    public class ContactsDataContext : DataContext
    {
        public Table<ContactInfo> Contacts;

        public ContactsDataContext(string path) : base(path) { }
    }
}
