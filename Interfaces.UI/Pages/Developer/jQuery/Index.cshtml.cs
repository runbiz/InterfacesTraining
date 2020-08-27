using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Interfaces.Core.DTO;
using Interfaces.Core.Logging;
using Interfaces.DAL.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Interfaces.UI.Pages.Developer.jQuery
{
    public class IndexModel : PageModel
    {
        #region DI/Constructor
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public IndexModel(IRepositoryWrapper repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        #endregion

        #region Page Model Properties
        [BindProperty]
        public DeveloperForCreationDto Dev { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        #endregion

        #region Page Methods
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetDevelopersAsync()
        {
            try
            {
                var devs = await _repository.Developer.GetAllDevelopersWithDetailsAsync();
                _logger.LogInfo($"Returned all developers from database.");

                var devsResult = _mapper.Map<IEnumerable<DeveloperDto>>(devs);
                return new JsonResult(devsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetDevelopers action: {ex.Message}");
                StatusMessage = "Error: Could not find developers.";
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> OnGetDepartmentsAsync()
        {
            try
            {
                var dpts = await _repository.Department.GetDepartmentsAsync();
                _logger.LogInfo($"Returned all departments from database.");

                var dptsResult = _mapper.Map<IEnumerable<DepartmentDto>>(dpts);
                return new JsonResult(dptsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetDepartments action: {ex.Message}");
                StatusMessage = "Error: Could not find departments.";
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError($"Invalid developer object sent from client.");
                    StatusMessage = "Warning: Some required fields are not valid.";
                    return Page();
                }

                var devEntity = _mapper.Map<Entities.Developer>(Dev);
                _repository.Developer.Create(devEntity);
                await _repository.SaveAsync();

                _logger.LogInfo($"Developer - {Dev.Name} was successfully created.");
                StatusMessage = $"Developer - {Dev.Name} was successfully created.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside OnPost action: {ex.Message}");
                StatusMessage = $"Error: Could not add developer {Dev.Name}.";
                return Page();
            }
        }

        public async Task<IActionResult> OnPutAsync(DeveloperDto model)
        {
            try
            {
                var devEntity = await _repository.Developer.GetDeveloperByIdAsync(model.Id);
                if (devEntity == null)
                {
                    _logger.LogError("Developer object sent from client could not be tied to an existing developer.");
                    return BadRequest("Developer is null");
                }

                _mapper.Map(model, devEntity);

                _repository.Developer.Update(devEntity);
                await _repository.SaveAsync();

                _logger.LogInfo($"Developer {model.Name} was successfully updated.");
                StatusMessage = $"Developer {model.Name} was successfully updated.";
                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside OnPutAsync action: {ex.Message}");
                StatusMessage = $"Error: Could not update developer {model.Name}.";
                return StatusCode(500, "Internal Server Error");
            }
        }

        public async Task<IActionResult> OnDeleteAsync(DeveloperDto model)
        {
            try
            {
                var devEntity = await _repository.Developer.GetDeveloperByIdAsync(model.Id);
                if (devEntity == null)
                {
                    _logger.LogError("Developer object sent from client could not be tied to an existing developer.");
                    return BadRequest("Developer is null");
                }                

                _repository.Developer.Delete(devEntity);
                await _repository.SaveAsync();

                _logger.LogInfo($"Developer {model.Name} was successfully deleted.");
                StatusMessage = $"Developer {model.Name} was successfully deleted.";
                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside OnDeleteAsync action: {ex.Message}");
                StatusMessage = $"Error: Could not deleted developer {model.Name}.";
                return StatusCode(500, "Internal Server Error");
            }
        }
        #endregion
    }
}