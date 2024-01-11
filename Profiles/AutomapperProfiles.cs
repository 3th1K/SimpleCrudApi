using AutoMapper;
using CrudApiAssignment.DTOs;
using CrudApiAssignment.Models;

namespace CrudApiAssignment.Profiles;

public class AutomapperProfiles : Profile
{
    public AutomapperProfiles()
    {
        CreateMap<User, UserResponse>();
    }
    
}
