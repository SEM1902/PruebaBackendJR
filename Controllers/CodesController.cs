using EnterpriseApi.Data;
using EnterpriseApi.DTOs;
using EnterpriseApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CodesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CodesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/codes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CodeDto>> GetCode(int id)
        {
            var code = await _context.Codes.FindAsync(id);

            if (code == null)
            {
                return NotFound();
            }

            return Ok(new CodeDto
            {
                Id = code.Id,
                OwnerId = code.OwnerId,
                Name = code.Name,
                Description = code.Description
            });
        }

        // POST: api/codes
        [HttpPost]
        public async Task<ActionResult<CodeDto>> PostCode(CreateCodeDto createDto)
        {
            // Validar si el owner existe
            var ownerExists = await _context.Enterprises.AnyAsync(e => e.Id == createDto.OwnerId);
            if (!ownerExists)
            {
                return BadRequest("El OwnerId especificado (Empresa) no existe.");
            }

            var code = new Code
            {
                OwnerId = createDto.OwnerId,
                Name = createDto.Name,
                Description = createDto.Description
            };

            _context.Codes.Add(code);
            await _context.SaveChangesAsync();

            var dto = new CodeDto
            {
                Id = code.Id,
                OwnerId = code.OwnerId,
                Name = code.Name,
                Description = code.Description
            };

            return CreatedAtAction(nameof(GetCode), new { id = code.Id }, dto);
        }

        // HttpPost por ID para cumplimiento literal del requerimiento "POST para modificar registros específicos"
        [HttpPost("{id}")]
        public async Task<IActionResult> PostModifyCode(int id, [FromBody] UpdateCodeDto updateDto)
        {
            return await PatchCode(id, updateDto);
        }

        // PATCH: api/codes/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchCode(int id, [FromBody] UpdateCodeDto updateDto)
        {
            var code = await _context.Codes.FindAsync(id);
            if (code == null)
            {
                return NotFound();
            }

            if (updateDto.OwnerId.HasValue)
            {
                var ownerExists = await _context.Enterprises.AnyAsync(e => e.Id == updateDto.OwnerId.Value);
                if (!ownerExists)
                {
                    return BadRequest("El OwnerId especificado no existe.");
                }
                code.OwnerId = updateDto.OwnerId.Value;
            }

            if (updateDto.Name != null) code.Name = updateDto.Name;
            if (updateDto.Description != null) code.Description = updateDto.Description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CodeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // --- ENDPOINTS ADICIONALES SOLICITADOS ---

        // Recuperar la información de la empresa dueña de un código, usando el id del código.
        // GET: api/codes/5/enterprise
        [HttpGet("{id}/enterprise")]
        public async Task<ActionResult<EnterpriseDto>> GetEnterpriseByCodeId(int id)
        {
            var code = await _context.Codes
                .Include(c => c.Owner)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (code == null)
            {
                return NotFound("Código no encontrado.");
            }

            if (code.Owner == null)
            {
                return NotFound("Empresa dueña no encontrada para este código.");
            }

            var enterpriseDto = new EnterpriseDto
            {
                Id = code.Owner.Id,
                Name = code.Owner.Name,
                Nit = code.Owner.Nit,
                Gln = code.Owner.Gln
            };

            return Ok(enterpriseDto);
        }

        private bool CodeExists(int id)
        {
            return _context.Codes.Any(e => e.Id == id);
        }
    }
}
