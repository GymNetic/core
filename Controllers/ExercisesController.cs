using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GYMNETIC.Core.Data;
using GYMNETIC.Core.Models;
using GYMNETIC.Core.Services;
using Newtonsoft.Json;

namespace GYMNETIC.Core.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExercisesController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IExerciseCategory _exerciseService;

    public ExercisesController(ApplicationDbContext db, IExerciseService exerciseService)
    {
        _db = db;
        _exerciseService = exerciseService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchExercises([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query não pode estar vazia.");

        var exercises = await _db.Exercises
            .Where(e => e.Name.Contains(query) || e.Description.Contains(query))
            .ToListAsync();

        if (!exercises.Any())
            return NotFound("Nenhum exercício encontrado.");

        return Ok(exercises);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetExerciseList()
    {
        var exercises = await _db.Exercises
            .OrderBy(e => e.Category)
            .ThenBy(e => e.Name)
            .Select(e => new
            {
                Id = e.Id,
                Name = e.Name,
                Category = e.Category,
                ImageUrl = e.ImageUrl
            })
            .ToListAsync();

        return Ok(exercises);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExercise(int id)
    {
        var exercise = await _db.Exercises
            .Include(e => e.MuscleGroups)
            .Include(e => e.Equipment)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (exercise == null)
            return NotFound();

        // Mapeamento para DTO para evitar ciclos
        var exerciseDto = new ExerciseDto
        {
            Id = exercise.Id,
            Name = exercise.Name,
            Description = exercise.Description,
            ImageUrl = exercise.ImageUrl,
            VideoUrl = exercise.VideoUrl,
            Category = exercise.Category,
            Difficulty = exercise.Difficulty,
            MuscleGroups = exercise.MuscleGroups?.Select(m => new MuscleGroupDto
            {
                Id = m.Id,
                Name = m.Name,
                ImageUrl = m.ImageUrl
            }).ToList() ?? new List<MuscleGroupDto>(),
            Equipment = exercise.Equipment?.Select(e => new EquipmentDto
            {
                Id = e.Id,
                Name = e.Name,
                ImageUrl = e.ImageUrl
            }).ToList() ?? new List<EquipmentDto>(),
            Steps = exercise.Steps?.Select(s => new ExerciseStepDto
            {
                Number = s.Number,
                Description = s.Description
            }).ToList() ?? new List<ExerciseStepDto>()
        };

        return Ok(exerciseDto);
    }

    [HttpGet("muscle-groups")]
    public async Task<IActionResult> GetAllMuscleGroups()
    {
        var muscleGroups = await _db.MuscleGroups.ToListAsync();
        var muscleGroupDtos = muscleGroups.Select(m => new MuscleGroupDto
        {
            Id = m.Id,
            Name = m.Name,
            ImageUrl = m.ImageUrl
        }).ToList();
        
        return Ok(muscleGroupDtos);
    }

    [HttpGet("equipment")]
    public async Task<IActionResult> GetAllEquipment()
    {
        var equipment = await _db.Equipment.ToListAsync();
        var equipmentDtos = equipment.Select(e => new EquipmentDto
        {
            Id = e.Id,
            Name = e.Name,
            ImageUrl = e.ImageUrl
        }).ToList();
        
        return Ok(equipmentDtos);
    }

    [HttpPost]
    public async Task<IActionResult> CreateExercise([FromBody] ExerciseCreateDto exerciseDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var exercise = new Exercise
        {
            Name = exerciseDto.Name,
            Description = exerciseDto.Description,
            ImageUrl = exerciseDto.ImageUrl,
            VideoUrl = exerciseDto.VideoUrl,
            Category = exerciseDto.Category,
            Difficulty = exerciseDto.Difficulty
        };

        // Adicionar grupos musculares
        if (exerciseDto.MuscleGroupIds != null && exerciseDto.MuscleGroupIds.Any())
        {
            exercise.MuscleGroups = await _db.MuscleGroups
                .Where(m => exerciseDto.MuscleGroupIds.Contains(m.Id))
                .ToListAsync();
        }

        // Adicionar equipamentos
        if (exerciseDto.EquipmentIds != null && exerciseDto.EquipmentIds.Any())
        {
            exercise.Equipment = await _db.Equipment
                .Where(e => exerciseDto.EquipmentIds.Contains(e.Id))
                .ToListAsync();
        }

        // Adicionar passos
        if (exerciseDto.Steps != null && exerciseDto.Steps.Any())
        {
            exercise.Steps = exerciseDto.Steps.Select(s => new ExerciseStep
            {
                Number = s.Number,
                Description = s.Description
            }).ToList();
        }

        _db.Exercises.Add(exercise);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetExercise), new { id = exercise.Id }, exercise);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExercise(int id, [FromBody] ExerciseCreateDto exerciseDto)
    {
        var exercise = await _db.Exercises
            .Include(e => e.MuscleGroups)
            .Include(e => e.Equipment)
            .Include(e => e.Steps)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (exercise == null)
            return NotFound();

        exercise.Name = exerciseDto.Name;
        exercise.Description = exerciseDto.Description;
        exercise.ImageUrl = exerciseDto.ImageUrl;
        exercise.VideoUrl = exerciseDto.VideoUrl;
        exercise.Category = exerciseDto.Category;
        exercise.Difficulty = exerciseDto.Difficulty;

        // Atualizar grupos musculares
        if (exerciseDto.MuscleGroupIds != null)
        {
            exercise.MuscleGroups.Clear();
            var muscleGroups = await _db.MuscleGroups
                .Where(m => exerciseDto.MuscleGroupIds.Contains(m.Id))
                .ToListAsync();
            exercise.MuscleGroups = muscleGroups;
        }

        // Atualizar equipamentos
        if (exerciseDto.EquipmentIds != null)
        {
            exercise.Equipment.Clear();
            var equipment = await _db.Equipment
                .Where(e => exerciseDto.EquipmentIds.Contains(e.Id))
                .ToListAsync();
            exercise.Equipment = equipment;
        }

        // Atualizar passos
        if (exerciseDto.Steps != null)
        {
            exercise.Steps.Clear();
            exercise.Steps = exerciseDto.Steps.Select(s => new ExerciseStep
            {
                Number = s.Number,
                Description = s.Description
            }).ToList();
        }

        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExercise(int id)
    {
        var exercise = await _db.Exercises.FindAsync(id);
        if (exercise == null)
            return NotFound();

        _db.Exercises.Remove(exercise);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    #region DTOs
    public class ExerciseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public string Category { get; set; }
        public string Difficulty { get; set; }
        public List<MuscleGroupDto> MuscleGroups { get; set; }
        public List<EquipmentDto> Equipment { get; set; }
        public List<ExerciseStepDto> Steps { get; set; }
    }

    public class ExerciseCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public string Category { get; set; }
        public string Difficulty { get; set; }
        public List<int> MuscleGroupIds { get; set; }
        public List<int> EquipmentIds { get; set; }
        public List<ExerciseStepDto> Steps { get; set; }
    }

    public class MuscleGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }

    public class EquipmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }

    public class ExerciseStepDto
    {
        public int Number { get; set; }
        public string Description { get; set; }
    }
    #endregion
}