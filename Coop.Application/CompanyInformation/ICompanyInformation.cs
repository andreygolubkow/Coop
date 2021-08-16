using System.Collections.Generic;

namespace Coop.Application.AdminNotes
{
    public interface ICompanyInformation
    {
        string GetCompany();

        List<string> GetCompanyProperties();

        List<CompanyContactViewModel> GetContacts();
    }
}