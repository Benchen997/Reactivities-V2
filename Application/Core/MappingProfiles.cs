using AutoMapper;
using Domain;

namespace Application.Core;

public class MappingProfiles: Profile
{ 
    // constructor
    public MappingProfiles()
    {
        // effectively maps the properties of the source object to the destination object
        CreateMap<Activity, Activity>();
    }
}