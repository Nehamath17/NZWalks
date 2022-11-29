using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper,
            IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
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
        [ActionName("GetWalkAsync")]
     
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
            // Validate incoming Request
            if (!(await ValidateAddWalkAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }

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
            [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            // Validate the incoming Request
            if (!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }

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

        #region Private methods

        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if (addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest),
                    $"{nameof(addWalkRequest)} Cannot be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name),
                    $"{nameof(addWalkRequest.Name)} Cannot be null or empty or white space.");
            }

            if (addWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length),
                    $"{nameof(addWalkRequest.Length)} should be greater than zero.");
            
            }
            var region = await regionRepository.GetAsync(addWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId),
                    $"{nameof(addWalkRequest.RegionId)} is invalid");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId),
                    $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid.");
                
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            if (updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest),
                    $"{nameof(updateWalkRequest)} Cannot be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name),
                    $"{nameof(updateWalkRequest.Name)} is required.");
            }

            if (updateWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Length),
                    $"{nameof(updateWalkRequest.Length)} should be greater than zero.");

            }
            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId),
                    $"{nameof(updateWalkRequest.RegionId)} is invalid");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId),
                    $"{nameof(updateWalkRequest.WalkDifficultyId)} is invalid.");

            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        

         }
    #endregion



    }

}
