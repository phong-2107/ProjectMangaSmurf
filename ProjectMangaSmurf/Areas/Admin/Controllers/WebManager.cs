using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectMangaSmurf.Data;
using ProjectMangaSmurf.Models;
using ProjectMangaSmurf.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectMangaSmurf.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuthScheme")]
    public class WebManagerController : Controller
    {
        private readonly ProjectDBContext _context;
        private readonly IWebMediaRepository _mediaRepository;
        private readonly ILogger<WebManagerController> _logger;

        public WebManagerController(ProjectDBContext context, ILogger<WebManagerController> logger, IWebMediaRepository mediaRepository)
        {
            _context = context;
            _logger = logger;
            _mediaRepository = mediaRepository;
        }

        #region Index
        [RBACAuthorize(PermissionId = 54)]
        public async Task<IActionResult> Index()
        {
            var configList = await _mediaRepository.GetAllConfigsAsync();
            return View(configList);
        }
        #endregion

        #region About
        public async Task<IActionResult> About()
        {
            var configList = new List<WebMediaConfig>();
            for (int id = 38; id <= 44; id++)
            {
                var config = await _mediaRepository.GetConfigByIdAsync(id);
                if (config != null)
                {
                    configList.Add(config);
                }
            }

            ViewBag.Configs = configList;
            return View();
        }
        #endregion

        #region SaveConfigValue
        [HttpPost]
        [RBACAuthorize(PermissionId = 55)]
        public async Task<IActionResult> SaveConfigValues([FromBody] List<WebMediaConfig> configs)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var config in configs)
                    {
                        var existingConfig = await _mediaRepository.GetConfigByIdAsync(config.IdConfig);
                        if (existingConfig != null)
                        {
                            existingConfig.ConfigValue = config.ConfigValue;
                            existingConfig.Active = config.Active;
                            await _mediaRepository.UpdateConfigAsync(existingConfig);
                        }
                    }
                    await transaction.CommitAsync();
                    return Ok();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, "An error occurred while saving the configurations.");
                }
            }
        }

        #endregion

        #region ToggleActive
        [HttpPost]
        [RBACAuthorize(PermissionId = 55)]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var config = await _mediaRepository.GetConfigByIdAsync(id);
            if (config == null)
            {
                return NotFound();
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    config.Active = !(config.Active ?? false);
                    await _mediaRepository.UpdateConfigAsync(config);
                    await transaction.CommitAsync();
                    return Ok();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        #endregion
    }
}
