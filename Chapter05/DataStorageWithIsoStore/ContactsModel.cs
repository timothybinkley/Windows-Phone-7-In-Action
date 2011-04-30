using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;

namespace DataStorage
{
    public class ContactsModel
    {
        private ObservableCollection<ContactInfo> contacts;
        public ReadOnlyObservableCollection<ContactInfo> Contacts { get; private set; }

        public ContactsModel()
        {
            contacts = new ObservableCollection<ContactInfo>();
            Contacts = new ReadOnlyObservableCollection<ContactInfo>(contacts);
        }

        public void LoadContacts()
        {
            // create the data storage folder
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!storage.DirectoryExists("ContactsFolder"))
                {
                    storage.CreateDirectory("ContactsFolder");
                }

                if (storage.FileExists(@"ContactsFolder\ContactsFile.data"))
                {
                    // load the data file into the contacts list.
                    using (IsolatedStorageFileStream stream =
                        storage.OpenFile(@"ContactsFolder\ContactsFile.data", FileMode.Open))
                    {
                        XmlSerializer serializer =
                            new XmlSerializer(typeof(List<ContactInfo>));
                        var contactList = (IList<ContactInfo>)serializer.Deserialize(stream);
                        foreach (var contact in contactList)
                        {
                            contacts.Add(contact);
                        }

                    }
                }
            }
        }

        public void SaveContacts()
        {
            using (IsolatedStorageFile storage =
                IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream =
                    storage.CreateFile(@"ContactsFolder\ContactsFile.data"))
                {
                    XmlSerializer serializer =
                        new XmlSerializer(typeof(ObservableCollection<ContactInfo>));
                    serializer.Serialize(stream, contacts);
                }
            }
        }

        public ContactInfo CreateContact()
        {
            var contact = new ContactInfo();
            contacts.Add(contact);
            return contact;
        }

        public void DeleteContact(ContactInfo contact)
        {
            contacts.Remove(contact);
        }

    }
}
