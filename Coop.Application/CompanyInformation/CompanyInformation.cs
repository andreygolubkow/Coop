using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;

namespace Coop.Application.AdminNotes
{
    public class CompanyInformation : ICompanyInformation
    {
        private readonly CompanyInformationOptions _options;


        public CompanyInformation(IOptions<CompanyInformationOptions> options)
        {
            _options = options.Value;
        }

        public string GetCompany()
        {
            return _options.Company;
        }

        public List<string> GetCompanyProperties()
        {
            return _options.Properties;
        }

        public List<CompanyContactViewModel> GetContacts()
        {
            return _options.Contacts.Select(p => new CompanyContactViewModel
            {
                Channel = p.Channel,
                Contact = p.Address
            }).ToList();
        }
    }
}