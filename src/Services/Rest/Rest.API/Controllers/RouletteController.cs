using Microsoft.AspNetCore.Mvc;
using Rest.API.Application.Adapters.RouletteDTOs;
using Rest.API.Application.Services.RouleteMS;
using Rest.API.Domain.AggregatesModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Rest.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        #region Variables

        private readonly IRouletteServices _services;

        #endregion

        #region Constructor

        public RouletteController(IRouletteServices services)
        {
            _services = services;
        }

        #endregion

        #region Gets

        /// <summary>
        /// List of roulettes
        /// </summary>
        /// <returns></returns>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<RouletteListDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            var result = await _services.ListAsync();
            return Ok(result);
        }

        #endregion

        #region Posts

        /// <summary>
        /// Register roulette
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Roulette), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] RouletteRegisterDTO item)
        {
            var result = await _services.AddAsync(item);
            return Ok(result);
        }
        
        /// <summary>
        /// Bet in roulette
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Roulette), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost("Bet")]
        public async Task<IActionResult> AddAsync([FromHeader(Name = "x-playerId")][Required] int playerId, [FromBody] RouletteBetRegisterDTO item)
        {
            var result = await _services.BetAsync(item, playerId);
            return Ok(result);
        }

        #endregion

        #region Puts

        /// <summary>
        /// open roulette
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPut("Open/{rouletteId}")]
        public async Task<IActionResult> OpenAsync(int rouletteId)
        {
            var result = await _services.OpenAsync(rouletteId);
            return Ok(result);
        }

        /// <summary>
        /// close roulette
        /// </summary>
        /// <param name="rouletteId"></param>
        /// <returns></returns>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<Board>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPut("Close/{rouletteId}")]
        public async Task<IActionResult> CloseAsync(int rouletteId)
        {
            var result = await _services.CloseAsync(rouletteId);
            return Ok(result);
        }

        #endregion
    }
}
