using System.Collections.Generic;
using ExportDemo.Models;

namespace ExportDemo.DataAccess
{
    public interface IPersonRepository
    {
        IEnumerable<Person> GetPersons();
    }
}
