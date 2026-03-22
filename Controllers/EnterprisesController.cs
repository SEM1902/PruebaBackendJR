using EnterpriseApi.Data;
using EnterpriseApi.DTOs;
using EnterpriseApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnterprisesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EnterprisesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/enterprises/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EnterpriseDto>> GetEnterprise(int id)
        {
            var enterprise = await _context.Enterprises.FindAsync(id);

            if (enterprise == null)
            {
                return NotFound();
            }

            return Ok(new EnterpriseDto
            {
                Id = enterprise.Id,
                Name = enterprise.Name,
                Nit = enterprise.Nit,
                Gln = enterprise.Gln
            });
        }

        // POST: api/enterprises
        [HttpPost]
        public async Task<ActionResult<EnterpriseDto>> PostEnterprise(CreateEnterpriseDto createDto)
        {
            var enterprise = new Enterprise
            {
                Name = createDto.Name,
                Nit = createDto.Nit,
                Gln = createDto.Gln
            };

            _context.Enterprises.Add(enterprise);
            await _context.SaveChangesAsync();

            var dto = new EnterpriseDto
            {
                Id = enterprise.Id,
                Name = enterprise.Name,
                Nit = enterprise.Nit,
                Gln = enterprise.Gln
            };

            return CreatedAtAction(nameof(GetEnterprise), new { id = enterprise.Id }, dto);
        }

        // HttpPost por ID para cumplimiento literal del requerimiento "POST para modificar registros específicos"
        [HttpPost("{id}")]
        public async Task<IActionResult> PostModifyEnterprise(int id, [FromBody] UpdateEnterpriseDto updateDto)
        {
            return await PatchEnterprise(id, updateDto);
        }

        // PATCH: api/enterprises/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchEnterprise(int id, [FromBody] UpdateEnterpriseDto updateDto)
        {
            var enterprise = await _context.Enterprises.FindAsync(id);
            if (enterprise == null)
            {
                return NotFound();
            }

            if (updateDto.Name != null) enterprise.Name = updateDto.Name;
            if (updateDto.Nit.HasValue) enterprise.Nit = updateDto.Nit;
            if (updateDto.Gln.HasValue) enterprise.Gln = updateDto.Gln.Value;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnterpriseExists(id))
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

        // Recuperar todas las empresas
        // GET: api/enterprises
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnterpriseDto>>> GetEnterprises()
        {
            var enterprises = await _context.Enterprises
                .Select(e => new EnterpriseDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Nit = e.Nit,
                    Gln = e.Gln
                })
                .ToListAsync();

            return Ok(enterprises);
        }

        // Recuperar todos los códigos pertenecientes a una empresa usando su id
        // GET: api/enterprises/5/codes
        [HttpGet("{id}/codes")]
        public async Task<ActionResult<IEnumerable<CodeDto>>> GetCodesByEnterpriseId(int id)
        {
            var enterpriseExists = await _context.Enterprises.AnyAsync(e => e.Id == id);
            if (!enterpriseExists)
            {
                return NotFound("Empresa no encontrada.");
            }

            var codes = await _context.Codes
                .Where(c => c.OwnerId == id)
                .Select(c => new CodeDto
                {
                    Id = c.Id,
                    OwnerId = c.OwnerId,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToListAsync();

            return Ok(codes);
        }

        // Recuperar una empresa con un nit especifico y sus códigos asociados.
        // GET: api/enterprises/by-nit/123456
        [HttpGet("by-nit/{nit}")]
        public async Task<ActionResult<EnterpriseWithCodesDto>> GetEnterpriseWithCodesByNit(long nit)
        {
            var enterprise = await _context.Enterprises
                .Include(e => e.Codes)
                .FirstOrDefaultAsync(e => e.Nit == nit);

            if (enterprise == null)
            {
                return NotFound("Empresa no encontrada con el Nit especificado.");
            }

            var result = new EnterpriseWithCodesDto
            {
                Id = enterprise.Id,
                Name = enterprise.Name,
                Nit = enterprise.Nit,
                Gln = enterprise.Gln,
                Codes = enterprise.Codes.Select(c => new CodeDto
                {
                    Id = c.Id,
                    OwnerId = c.OwnerId,
                    Name = c.Name,
                    Description = c.Description
                }).ToList()
            };

            return Ok(result);
        }

        private bool EnterpriseExists(int id)
        {
            return _context.Enterprises.Any(e => e.Id == id);
        }
    }
}
