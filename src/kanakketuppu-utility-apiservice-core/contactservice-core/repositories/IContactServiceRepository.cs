using System.Collections.Generic;
using KanakketuppuUtilityApiServiceCore.ContactServiceCore.Datacontracts.DAOs;
using KanakketuppuUtilityApiServiceCore.DataContracts.Commons;
using KanakketuppuUtilityApiServiceModel.ContactApiServiceModels.Contact.GetContact;

namespace KanakketuppuUtilityApiServiceCore.ContactServiceCore.Repositories
{
    public interface IContactServiceRepository
    {
        void InsertContact(ContactDAO contactDAO);

        IEnumerable<ContactModel> GetContactsModel();

        IEnumerable<ContactModel> GetContactModel(long contactId);

        IEnumerable<ContactDAO> GetContactById(long parsedId);

        void DeleteContactById(ContactDAO contactDAO);
    }
}