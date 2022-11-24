using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            //Fetch data from database - domain walks
            var walksDomains = await walkRepository.GetAllAsync();

            //Convert domain walks to DTO walks
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomains);

            //Return response
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //Get walk Domain object from database
            var walkDomains = await walkRepository.GetAsync(id);

            //Convert domain object to DTO 
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomains);

            //Return response
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            //Convert DTO to Domain Object
            var walksDomains = new Models.Domains.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId 
            };

            //Pass domain object to Respository to persist this
            walksDomains = await walkRepository.AddAsync(walksDomains);

            //Convert the Domain object back to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walksDomains.Id,
                Length = walksDomains.Length,
                Name =  walksDomains.Name,
                RegionId = walksDomains.RegionId,
                WalkDifficultyId = walksDomains.WalkDifficultyId    
            };

            //send DTO response back to client
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]

        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id,
            [ FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //Convert DTO to Domain object
            var walkDomains = new Models.Domains.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };
            // Pass details to Respository - Get Doamain objects in response (or null)
            walkDomains = await walkRepository.UpdateAsync(id, walkDomains);

            //Handle Null (not Found)
            if (walkDomains == null)
            {
                return NotFound();
            }

            //Convert back Domain to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomains.Id,
                Length = walkDomains.Length,
                Name = walkDomains.Name,
                RegionId = walkDomains.RegionId,
                WalkDifficultyId = walkDomains.WalkDifficultyId
            };

            //return Response
            return Ok(walkDTO);
        

        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteWalkAsync( Guid id)
        {
            // call Repository to delete walk
            var walkDomains = await walkRepository.DeleteAsync(id);

            if (walkDomains == null)
            {
                return NotFound();
            }

            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomains);

            return Ok(walkDTO);
        }



      }

}
