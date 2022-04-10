using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.BLL
{
    public class HostService
    {
        public IHostRepository HostRepository;

        public HostService(IHostRepository repo)
        {
            HostRepository = repo;
        }
        
        public Result<Host> FindByEmail(string email)
        {
            Result<List<Host>> hosts = HostRepository.GetAll();
            Result<Host> result = new Result<Host>();

            result.Data = hosts.Data.Where(host => host.Email == email).SingleOrDefault();
            if (result.Data == null)
            {
                result.Success = false;
                result.Message = $"No host found with email: {email}.";
            }
            else
            {
                result.Success = true;
                result.Message = $"Host with {email} found.";
            }
            
            return result;
        }
        public Result<List<Host>> FindByLastName(string prefix)
        {
            Result<List<Host>> result = new Result<List<Host>>();
            result.Data = HostRepository.GetAll().Data
                    .Where(host => host.LastName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            if(result.Data.Count == 0)
            {
                result.Success = false;
                result.Message = $"No host's last name starts with \"{prefix}\".";
            }
            else
            {
                result.Success = true;
                result.Message = "Hosts whose last names starts with \"{prefix}\" were found.";
            }
            return result;
        }
    }
}