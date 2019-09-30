using System;
using System.Collections.Generic;
using KanakketuppuUtilityApiServiceCore.ContactServiceCore.Datacontracts;
using KanakketuppuUtilityApiServiceCore.ContactServiceCore.Mappers;
using KanakketuppuUtilityApiServiceCore.ContactServiceCore.PostProcessors;
using KanakketuppuUtilityApiServiceCore.ContactServiceCore.Processors;
using KanakketuppuUtilityApiServiceCore.ContactServiceCore.Repositories;
using KanakketuppuUtilityApiServiceCore.ContactServiceCore.Utility;
using KanakketuppuUtilityApiServiceCore.ContactServiceCore.Validations;
using KanakketuppuUtilityApiServiceCore.ContactServiceCore.Verifiers;
using KanakketuppuUtilityApiServiceCore.DataContracts.Commons;
using KanakketuppuUtilityApiServiceCore.Utility;
using KanakketuppuUtilityApiServiceModel.ContactApiServiceModels.Contact.GetContact;
using KatavuccolCommon.Extensions;

namespace KanakketuppuUtilityApiServiceCore.ContactServiceCore.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactServiceValidation contactServiceValidation;
        private readonly IContactServiceVerifier contactServiceVerifier;
        private readonly IContactServiceProcessor contactServiceProcessor;
        private readonly IContactServicePostProcessor contactServicePostProcessor;
        private readonly IContactServiceMapper contactServiceMapper;
        private readonly IContactServiceRepository contactServiceRespostry;

        public ContactService(
            IContactServiceValidation contactServiceValidation,
            IContactServiceVerifier contactServiceVerifier,
            IContactServiceProcessor contactServiceProcessor,
            IContactServicePostProcessor contactServicePostProcessor,
            IContactServiceMapper contactServiceMapper,
            IContactServiceRepository contactServiceRespostry)
        {
            this.contactServiceValidation = contactServiceValidation;
            this.contactServiceVerifier = contactServiceVerifier;
            this.contactServiceProcessor = contactServiceProcessor;
            this.contactServicePostProcessor = contactServicePostProcessor;
            this.contactServiceMapper = contactServiceMapper;
            this.contactServiceRespostry = contactServiceRespostry;
        }

        public List<ErrorMessage> CreateContact(CreateContactMsgEntity createContactMsgEntity)
        {
            //Setup
            createContactMsgEntity.CreatedOn = createContactMsgEntity.ModifiedOn = DateTime.UtcNow;
            createContactMsgEntity.CreatedBy = createContactMsgEntity.ModifiedBy = "ADMIN";

            //Validation
            var resultMessage = contactServiceValidation.ValidCreateContact(createContactMsgEntity);
            if (resultMessage.AnyWithNullCheck())
                return resultMessage;

            //Verifier
            resultMessage = contactServiceVerifier.VerifyCreateContact(createContactMsgEntity);
            if (resultMessage.AnyWithNullCheck())
                return resultMessage;

            //Processor
            resultMessage = contactServiceProcessor.ProcessorCreateContact(createContactMsgEntity);
            if (resultMessage.AnyWithNullCheck())
                return resultMessage;

            //PostProcessor
            var postResultMessage = contactServicePostProcessor.PostProcessorCreateContact(createContactMsgEntity);

            return resultMessage;
        }

        public ContactModel GetContactModel(string id)
        {
            long contactId = id.ToLong();
          
            //var contactId = long.Parse(id);
            return contactServiceRespostry.GetContactModel(contactId);
        }

        public IEnumerable<ContactModel> GetContactsModel()
        {
            return contactServiceRespostry.GetContactsModel();
        }
    }
}