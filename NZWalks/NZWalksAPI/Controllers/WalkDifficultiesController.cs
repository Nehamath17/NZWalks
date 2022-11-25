using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

      public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
       {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllwalkDifficulties()
        {
            var walkDifficultiesDomains= await walkDifficultyRepository.GetAllAsync();

            // Convert Domain to dtos
            var walkDifficultiesDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficultiesDomains);
           
            return Ok(walkDifficultiesDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyById")]

        public async Task<IActionResult> GetWalkDifficultyById(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.GetAsync(id);
            if (walkDifficulty == null)
            {
                return NotFound();
            }

            //Convert Domain to DTOs
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync(
            Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            // Convert DTO to domain model
            var walkDifficultyDomains = new Models.Domains.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code
            };
            // Call repository
            walkDifficultyDomains = await walkDifficultyRepository.AddAsync(walkDifficultyDomains);

            //Convert Doamain to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomains);

            //Return response]
            return CreatedAtAction(nameof(GetWalkDifficultyById),
                new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }

            [HttpPut]
            [Route("{id:guid}")]

            public async Task<IActionResult> UpdateWalkDifficultyAsync(Guid id,
                Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            // Convert DTO to domain model
            var walkDifficultyDomains = new Models.Domains.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code
            };
            // Call repository to update
            walkDifficultyDomains = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomains);

            if (walkDifficultyDomains == null)
            {
                return NotFound();
            }

            //Convert Doamain to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomains);

            //Return response]
            return Ok(walkDifficultyDTO);
             
        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteWalkDifficulty(Guid id)
        {
             var walkDifficultyDomains = await walkDifficultyRepository.DeleteAsync(id);
            if (walkDifficultyDomains == null)
            {
                return NotFound();
            }
            //Convert to DTO

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomains);
            return Ok(walkDifficultyDTO);
        }


        }
     }

